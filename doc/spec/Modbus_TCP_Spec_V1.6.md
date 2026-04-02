# Modbus TCP 通訊規格書 (EAP V1.0.3 標準) - 版本 V1.6

本規格書嚴格對照 V1.0.3 JSON 結構，並針對長字串（長 Barcode, 40碼 RecipeID）進行長度優化與流程連續位址規劃。

## 1. 狀態與控制位元組 (Packed Bits)
| 位址 | 名稱 | 方向 | Bit 定義 |
| :--- | :--- | :--- | :--- |
| **40001** | **PLC Status Word** | PLC→EAP | Bit0:HBT, Bit1:TimeSync, Bit2:StatusQuery, Bit3:UserVerify, Bit4:TrackIn, Bit5:TrackOut, Bit6:Ready, Bit7:Alarm |
| **40002** | **EAP Control Word** | EAP→PLC | Bit1:TimeSyncACK, Bit2:StatusACK, Bit3:UserVerifyACK, Bit4:TrackInACK, Bit5:TrackOutACK, Bit6:Stop, Bit7:Start |

---

## 2. 流程資料專區 (1:1 JSON Mapping & Extended Length)

### 2.1 人員驗證專區 (UserVerify)
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40010 | 10w | PLC→EAP | **[R]** UserID (工號) |
| 40020 | 40w | PLC→EAP | **[R]** Barcode (人員長代碼，支援至 80 字元) |
| 40060 | 20w | EAP→PLC | **[W]** ResultMsg (驗證回覆訊息) |

### 2.2 進站報工專區 (Track-In)
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40100 | 10w | PLC→EAP | **[R]** UserID (報工工號) |
| 40110 | 10w | PLC→EAP | **[R]** CassetteID (載具編號，支援 14 碼) |
| 40120 | 10w | PLC→EAP | **[R]** WorkOrder (工單號碼，支援 12 碼) |
| 40130 | 10w | PLC→EAP | **[R]** MaterialID (材料ID) |
| 40140 | 10w | PLC→EAP | **[R]** ToolingID (治具ID) |
| 40150 | 10w | PLC→EAP | **[R]** PanelID (板號) |
| 40160 | 1w | PLC→EAP | **[R]** Qty (進站數量) |
| 40170 | 20w | EAP→PLC | **[W]** RecipeID (MES指派程式，支援 40 碼) |
| 40190 | 20w | EAP→PLC | **[W]** ResultMsg (進站回覆訊息) |

### 2.3 出站報工專區 (Track-Out)
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40200 | 10w | PLC→EAP | **[R]** UserID (報工工號) |
| 40210 | 10w | PLC→EAP | **[R]** CassetteID (載具編號) |
| 40220 | 10w | PLC→EAP | **[R]** WorkOrder (工單號碼) |
| 40230 | 1w | PLC→EAP | **[R]** Qty (產出數量) |
| 40231 | 10w | PLC→EAP | **[R]** Result (PASS/FAIL) |
| 40241 | 10w | PLC→EAP | **[R]** NGCode (不良代碼) |
| 40260 | 20w | EAP→PLC | **[W]** ResultMsg (出站回覆訊息) |

### 2.4 狀態變更、警報與數據 (Rolling Trigger)
| 觸發位址 | ACK位址 | 資料位址 | 長度 | 說明 |
| :--- | :--- | :--- | :--- | :--- |
| 40003 (Rolling) | 41003 (ACK) | 40300 | 1w | NewStatus |
| - | - | 40301 | 1w | OldStatus |
| - | - | 40302 | 10w | ReasonCode |
| 40001 (Rolling) | 41001 (ACK) | 40400 | 10w | AlarmCode |
| - | - | 40410 | 20w | AlarmText |
| 40002 (Rolling) | 41002 (ACK) | 40500 | 10w | WorkOrder (數據關聯) |
| - | - | 40510 | 20w | DataPayload (數據內容) |

---
**版本**: V1.6 | **日期**: 2026-03-30
