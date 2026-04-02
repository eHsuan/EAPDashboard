# Modbus TCP 通訊規格書 (EAP V1.0.3 標準) - 版本 V2.1

本規格書支援「整線群組 (In-Line Group)」架構，允許多個站點 (Station) 透過單一 PLC 窗口與 EAP 進行隔離通訊。

## 1. 站點位址劃分原則 (Station Offsets)
每台站點分配 100 個 Word 空間，確保資料區塊完全獨立。
*   Station 1 (Loader): 40010 - 40099
*   Station 2 (Process): 40100 - 40199
*   Station 3 (Unloader): 40200 - 40299

## 2. 觸發與控制字組 (Line Level)
所有站點的觸發 Bit 集中於此，實現高效掃描。

### 2.1 PLC -> EAP 狀態字組 (位址 40001)
| Bit | 名稱 | 說明 | 歸屬站點 |
| :--- | :--- | :--- | :--- |
| Bit 0 | Heartbeat | 心跳位元 | 全線 |
| Bit 3 | L_UserVerifyReq | 人員驗證請求 | Station 1 |
| Bit 4 | L_TrackInReq | 進站報工請求 | Station 1 |
| Bit 5 | UL_TrackOutReq | 出站報工請求 | Station 3 |
| Bit 6 | Dev_DataTrigger | 顯影數據觸發 (如不用Rolling) | Station 2 |

### 2.2 EAP -> PLC 控制字組 (位址 40002)
| Bit | 名稱 | 說明 | 對應 Bit |
| :--- | :--- | :--- | :--- |
| Bit 3 | L_UserVerifyACK | 驗證完成 | 40001.3 |
| Bit 4 | L_TrackInACK | 進站完成 | 40001.4 |
| Bit 5 | UL_TrackOutACK | 出站完成 | 40001.5 |

---

## 3. 詳細資料區 (Station Buffer)

### 3.1 站點 1 (投板機)
| 位址 | 長度 | 類型 | 說明 |
| :--- | :--- | :--- | :--- |
| 40010 | 40w | ASCII | Barcode (人員代碼) |
| 40050 | 10w | ASCII | Cassette ID (載具) |
| 40060 | 10w | ASCII | WorkOrder (工單) |
| 40070 | 1w  | WORD | ResultCode (1:OK, 2:NG) |

### 3.2 站點 3 (收板機)
| 位址 | 長度 | 類型 | 說明 |
| :--- | :--- | :--- | :--- |
| 40200 | 10w | ASCII | Cassette ID (載具) |
| 40210 | 10w | ASCII | WorkOrder (工單) |
| 40220 | 1w  | WORD | Qty (產出數量) |
| 40230 | 1w  | WORD | ResultCode (1:OK, 2:NG) |

---

## 4. 業務邏輯對接
*   **統一上報**：EAP 偵測到任何站點的請求，統一以 SQLite 中定義的 `EqpId` (Group ID) 作為 MES JSON 的報工主體。
*   **隔離交握**：不同站點的觸發 Bit 彼此獨立，支援「投板機進站」與「收板機出站」同時進行而不產生干擾。

---
**版本**: V2.1 | **日期**: 2026-03-30
