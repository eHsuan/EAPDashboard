# 📋 TP 設備出站計算與良率檢核測試計畫 (TP Calculation & Yield Test Plan)

本文件詳細定義 EAPDashboard 針對 TP 系列（Taping & Packaging / 包裝編帶）設備之出站計算、良率檢核、NG 代碼平攤扣除演算法、完整通訊時序流程以及測試情境矩陣。

---

## ▍ 1. 核心業務通訊生命週期流程圖 (End-to-End Sequence)

```mermaid
sequenceDiagram
    autonumber
    actor EQ as 機台 / EQSimulator
    participant Broker as MQTT Broker
    participant EAP as EAPDashboard
    participant Cache as 本機快取 (Cache / DB)
    participant MES as MES WebService

    %% 1. 進站階段
    rect rgb(240, 248, 255)
    Note over EQ, MES: 【階段一】工單進站 (TrackIn)
    EQ->>Broker: 發布 TrackInReq (WorkOrder, RecipeID, UserID)
    Broker->>EAP: 轉發至 machine/{EQID}/Report
    EAP->>MES: 呼叫 WOCHECKIN (驗證進站)
    MES-->>EAP: 回傳進站結果 (PASS)
    EAP->>MES: 呼叫 WOQRYRequest (查詢工單資訊)
    MES-->>EAP: 回傳工單投入數 (InputQty)
    EAP->>Cache: 儲存 InputQty 至 input_qty_cache.json
    EAP->>Broker: 發布 ReplyTrackInReq (PASS)
    Broker->>EQ: 轉發至 machine/{EQID}/Command
    end

    %% 2. 加工與事件上報階段
    rect rgb(255, 250, 240)
    Note over EQ, MES: 【階段二】加工過程資料上報 (Reports & Events)
    EQ->>Broker: 上報 LEFTOVER_REPORT (剩料數量 LeftoverQty)
    Broker->>EAP: 轉發 LEFTOVER_REPORT
    EAP->>Cache: 持久化剩料資訊至 leftovers_cache.json
    EAP-->>EQ: 回覆 ACK

    EQ->>Broker: 上報 TOTAL_COUNT_REPORT (盤數 ReelCount, 每盤顆數 QtyPerReel)
    Broker->>EAP: 轉發 TOTAL_COUNT_REPORT
    EAP->>Cache: 持久化總數資訊至 total_count_cache.json
    EAP-->>EQ: 回覆 ACK

    opt 產出補料 (可選)
        EQ->>Broker: 上報 PRODUCT_LEFTOVER_REPORT (補料數量 AddQty)
        Broker->>EAP: 轉發 PRODUCT_LEFTOVER_REPORT
        EAP->>Cache: 持久化補料資訊至 product_leftovers_cache.json
        EAP-->>EQ: 回覆 ACK
    end

    loop 不良品上報
        EQ->>Broker: 上報 NGCODE_REPORT (NGCode, NGName, NGQty)
        Broker->>EAP: 轉發 NGCODE_REPORT
        EAP->>MES: 呼叫 EqpWONGAdd (上報單筆不良)
        MES-->>EAP: 回傳上報結果
        EAP->>Cache: 記錄並持久化至 ng_code_cache.json
        EAP-->>EQ: 回覆 ACK
    end
    end

    %% 3. 出站與計算階段
    rect rgb(240, 255, 240)
    Note over EQ, MES: 【階段三】工單出站與平攤結算 (TrackOut & Settlement)
    EQ->>Broker: 發布 TrackOutReq (WorkOrder, Qty, UserID)
    Broker->>EAP: 轉發 TrackOutReq
    EAP->>Cache: 讀取 InputQty, 總數, 剩料, 補料, NGCode 清單
    Note over EAP: 執行出站檢核與平攤扣除演算法:<br/>1. 檢核總數與剩料報告<br/>2. 計算 PassQty, CalcNG, Yield<br/>3. 檢核良率 (95%~100%) 與 NG數溢出<br/>4. 平攤扣除非特殊 NG 數量 (QC/免補料不扣)
    alt 檢核失敗 (缺報 / 良率異常 / NG數溢出)
        EAP->>Broker: 發布 ReplyTrackOutReq (FAIL, 錯誤原因)
        Broker->>EQ: 轉發 ReplyTrackOutReq (阻擋出站)
    else 檢核通過
        EAP->>MES: 呼叫 WOCHECKOUT (OutPut = PassQty, UserID)
        MES-->>EAP: 回傳出站結果 (PASS)
        EAP->>Broker: 發布 ReplyTrackOutReq (PASS, OK)
        Broker->>EQ: 轉發 ReplyTrackOutReq (出站完成)
    end
    end
```

---

## ▍ 2. 核心計算公式與架構設計

### 2.1 良品數與 NG 數計算公式
$$\text{PassQty} = (\text{ReelCount} \times \text{QtyPerReel}) + \text{LeftoverQty} - \text{ProductLeftoverAddQty}$$
$$\text{CalcNG} = \text{InputQty} - \text{PassQty}$$
$$\text{Yield} = \frac{\text{PassQty}}{\text{InputQty}} \times 100\%$$

* **參數定義**：
  * **$\text{InputQty}$ (投入數)**：進站時透過 MES `WOQRYRequest` 查詢取得之工單投入數（儲存於快取 `input_qty_cache.json`）。
  * **$\text{ReelCount}$ (盤數)**：機台透過總數上報事件（`TOTAL_COUNT_REPORT`）上報之已編帶盤數。
  * **$\text{QtyPerReel}$ (每盤顆數)**：機台透過總數上報事件上報之標準單盤數量。
  * **$\text{LeftoverQty}$ (剩料數)**：機台透過剩料上報事件（`LEFTOVER_REPORT`）上報之當批尾數剩料。
  * **$\text{ProductLeftoverAddQty}$ (產出補料數)**：機台透過產出補料事件（`PRODUCT_LEFTOVER_REPORT`）上報之補料數（預設為 0）。
  * **$\text{PassQty}$ (計算良品數)**：出站時由 EAP 自動計算得出，並自動覆寫報工給 MES `WOCHECKOUT` 的 `torQty`。
  * **$\text{CalcNG}$ (計算不良品數)**：出站時由投入數減去良品數得出，作為 MES 報工之不良總數。

---

## ▍ 3. 出站檢核與判定決策流程圖 (Validation Decision Flow)

```mermaid
flowchart TD
    Start([機台發起 TrackOutReq]) --> LoadData[自 Cache 載入投入數、總數、剩料、補料與 NG 清單]
    
    LoadData --> CheckInput{是否有投入數紀錄?}
    CheckInput -- 否 (0) --> SetFallbackInput[以機台上報 Qty 或總產出暫代]
    CheckInput -- 是 --> CheckReports{是否已上報<br/>總數與剩料報告?}
    SetFallbackInput --> CheckReports

    CheckReports -- 否 --> ErrMiss[阻擋出站: 回覆缺少總數或剩料報告]
    CheckReports -- 是 --> CalcMath["計算:
    PassQty = (ReelCount * QtyPerReel) + Leftover - AddQty
    CalcNG = InputQty - PassQty
    Yield = PassQty / InputQty"]

    CalcMath --> CheckYieldLow{Yield < MinYieldRate?<br/>預設 < 95%}
    CheckYieldLow -- 是 --> ErrYieldLow[阻擋出站: 回覆良率過低異常]
    CheckYieldLow -- 否 --> CheckYieldHigh{Yield > MaxYieldRate?<br/>預設 > 100%}
    CheckYieldHigh -- 是 --> ErrYieldHigh[阻擋出站: 回覆良率過高異常]
    
    CheckYieldHigh -- 否 --> FilterNG[篩選 NG 項目:<br/>特殊項目: QC Keep Sample, 免補料<br/>非特殊項目: 其他一般缺陷]
    FilterNG --> CheckOverflow{特殊 NG 總和 > CalcNG?}
    CheckOverflow -- 是 --> ErrOverflow[阻擋出站: NG數溢出需人員介入]

    CheckOverflow -- 否 --> CheckNeedDeduct{CalcNG < 上報 NG 總和?}
    CheckNeedDeduct -- 否 --> ExecMesCheckout[呼叫 MES WOCHECKOUT 報工<br/>OutPut = PassQty]
    CheckNeedDeduct -- 是 --> ExecEvenDeduct[執行非特殊 NG 平準化公平平攤扣除演算法]
    ExecEvenDeduct --> ExecMesCheckout

    ExecMesCheckout --> CheckMesReply{MES 是否回傳 PASS?}
    CheckMesReply -- 是 --> ReplyPass[回覆機台 ReplyTrackOutReq: PASS]
    CheckMesReply -- 否 --> ReplyFail[回覆機台 ReplyTrackOutReq: FAIL 與 MES 錯誤]

    ErrMiss --> ReplyReject[回覆機台 ReplyTrackOutReq: FAIL]
    ErrYieldLow --> ReplyReject
    ErrYieldHigh --> ReplyReject
    ErrOverflow --> ReplyReject
```

---

## ▍ 4. 非特殊 NG 剩餘量平準化演算法流程圖 (Water-Level Remaining Balance Allocation)

```mermaid
flowchart TD
    DeductStart([開始 NG 平準化修約]) --> InitRemaining["計算目標剩餘總額:
    NonSpecialSum = sum(非特殊項目原始數量)
    Diff = TotalReportedNG - CalcNG
    目標剩餘總數 targetRemainingTotal = NonSpecialSum - Diff
    待分配剩餘數 remainingToAllocate = targetRemainingTotal"]
    
    InitRemaining --> InitAlloc["初始化各非特殊項目剩餘量:
    remainingAllocations[item] = 0"]

    InitAlloc --> LoopStart{remainingToAllocate > 0.0001?}
    
    LoopStart -- 否 (目標剩餘量分配完畢) --> ApplyChanges["計算各項目扣除額並回寫:
    newQty = remainingAllocations[item]
    deduct = OriginalQty - newQty
    item.NGQty = newQty"]
    
    LoopStart -- 是 --> FindEligible["找出尚未達原始上限的項目:
    eligible = items where remainingAllocations < OriginalQty"]

    FindEligible --> CheckEligibleCount{eligible 數量 > 0?}
    CheckEligibleCount -- 否 (所有項目皆達上限) --> ApplyChanges
    CheckEligibleCount -- 是 --> FindLowest["取得當前最低水位 minAlloc 與最低水位項目群 lowestGroup"]

    FindLowest --> CalcStep["計算本輪每項分配增量:
    step = floor(remainingToAllocate / lowestGroup.Count)
    若 step <= 0 則設為 1 (輪流分配 1 顆)"]

    CalcStep --> DistributeLevel["依序向 lowestGroup 各項目注入剩餘量:
    maxAdd = OriginalQty - remainingAllocations[item]
    toAdd = min(maxAdd, step, remainingToAllocate)
    remainingAllocations[item] += toAdd
    remainingToAllocate -= toAdd"]
    
    DistributeLevel --> LoopStart

    ApplyChanges --> SaveDisk[儲存更新至 ng_code_cache.json 磁碟快取]
    SaveDisk --> DeductEnd([修約完成，進入 MES 報工])
```

---

## ▍ 5. 測試情境矩陣 (Test Scenarios Matrix)

| 編號 | 測試情境 | 投入數 (Input) | 上報參數 (總數/剩料/補料) | 上報 NG 清單 (Reported NG) | 預期計算 PassQty / NG | 預期結果與行為 |
| :---: | :--- | :---: | :--- | :--- | :---: | :--- |
| **TC-01** | **正常出站 (剩餘量平準化修約)** | 1000 | 盤數: 9, 單盤: 100 (900)<br>剩料: 90, 補料: 10 | QC Keep Sample: 5<br>免補料: 5<br>UPNG501 (Card): 20<br>UPNG502 (Broken): 10 | $\text{PassQty} = 980$<br>$\text{CalcNG} = 20$<br>$\text{Yield} = 98.0\%$ | **PASS 出站成功**<br>Diff = 20，目標剩餘 = 10。<br>特殊項目不扣 (各5)。<br>UPNG501 扣 15 ($\rightarrow 5$)，UPNG502 扣 5 ($\rightarrow 5$)。<br>兩者剩餘數量皆平準為 5 顆。<br>MES OutPut = 980。 |
| **TC-02** | **缺報總數或剩料** | 1000 | 未發送總數或剩料上報 | DEF1: 10 | 無法計算 | **FAIL 阻擋出站**<br>回覆 `Missing TotalCountReport / LeftoversReport`。 |
| **TC-03** | **良率過低異常 (<95%)** | 1000 | 盤數: 8, 單盤: 100 (800)<br>剩料: 0, 補料: 0 | DEF1: 200 | $\text{PassQty} = 800$<br>$\text{CalcNG} = 200$<br>$\text{Yield} = 80.0\%$ | **FAIL 阻擋出站**<br>回覆 `Yield rate (80.0%) below minimum limit (95.0%)`。 |
| **TC-04** | **良率過高異常 (>100%)** | 1000 | 盤數: 11, 單盤: 100 (1100)<br>剩料: 0, 補料: 0 | 無 | $\text{PassQty} = 1100$<br>$\text{CalcNG} = -100$<br>$\text{Yield} = 110.0\%$ | **FAIL 阻擋出站**<br>回覆 `Yield rate (110.0%) above maximum limit (100.0%)`。 |
| **TC-05** | **特殊 NG 數溢出** | 1000 | 盤數: 9, 單盤: 100 (900)<br>剩料: 80, 補料: 0 | QC Keep Sample: 25<br>免補料: 0 | $\text{PassQty} = 980$<br>$\text{CalcNG} = 20$ | **FAIL 阻擋出站**<br>SpecialNGSum (25) > CalcNG (20)，回覆 `NG數溢出，需要人員介入處理`。 |
| **TC-06** | **多項目含小額缺陷平準化** | 1000 | 盤數: 9, 單盤: 100 (900)<br>剩料: 93, 補料: 10 | QC Keep Sample: 5<br>SMALL: 2<br>BIG1: 20<br>BIG2: 10 | $\text{PassQty} = 983$<br>$\text{CalcNG} = 17$ | **PASS 出站成功**<br>Diff = 20，目標剩餘 = 12。<br>QC 不扣 (5)。<br>SMALL 保留上限 2 (扣 0)，BIG1 扣 15 ($\rightarrow 5$)，BIG2 扣 5 ($\rightarrow 5$)。<br>MES OutPut = 983。 |
| **TC-07** | **重啟持久化快取驗證** | 1000 | 上報總數、剩料、NG 代碼 | 各類 NG | 相同 PassQty | **PASS 驗證通過**<br>EAP 重啟後讀取 `Cache/*.json` 快取，各項數據不遺失並順利完成出站。 |

---

## ▍ 6. EQ 模擬器操作驗證步驟 (Simulator Guide)

### 步驟 1：啟動 EAPDashboard 與 EQSimulator
1. 執行 `EAPDashboard.exe`，確認 MQTT Broker 狀態指示燈為綠色 Connected。
2. 啟動 `EQSimulator.exe`，設定機台 ID（如 `TP0013`）與 Broker 資訊，點擊 **「連線」**。

### 步驟 2：執行情境測試（以情境 1 為例）
1. 在 EQ 模擬器下方 **「TP 測試情境」** 下拉選單中選擇 `Scenario 1: 正常出站 (投入1000 / 良品980 / 良率98%)`。
2. 點擊 **「載入情境參數」**（此時機台 ID 與 MQTT 連線參數保持不變，業務參數自動填入）。
3. 依序點擊下方操作按鈕：
   * 點擊 **`TrackIn`** ➜ MES 進站成功。
   * 點擊 **`剩料上報`** ➜ 記錄尾數 80 顆。
   * 點擊 **`總數上報`** ➜ 記錄 9 盤 $\times$ 100 顆 = 900 顆。
   * 點擊 **`上報全部 NG`** ➜ 上報 4 筆缺陷項目。
   * 點擊 **`TrackOut`** ➜ EAP 執行平攤計算並覆寫 `torQty = 980`，MES 出站成功。
4. 觀察 EAP 儀表板與日誌視窗，確認出站良品數、扣減明細與 MES 回應完全相符。
