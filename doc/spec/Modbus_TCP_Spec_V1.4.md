# Modbus TCP 通訊規格書 (EAP V1.0.3 標準) - 版本 V1.4

本規格書針對長字串資料（長 Barcode, RecipeID）進行了長度優化，確保能完整容納 60 碼以上的人員代碼。

## 1. 狀態與控制位元組 (Packed Bits)
| 位址 | 名稱 | 方向 | Bit 定義 |
| :--- | :--- | :--- | :--- |
| **40001** | **PLC Status Word** | PLC→EAP | Bit0:HBT, Bit1:TimeSync, Bit2:Status, Bit3:UserVerify, Bit4:TrackIn, Bit5:TrackOut, Bit6:Ready, Bit7:Alarm |
| **40002** | **EAP Control Word** | EAP→PLC | Bit1:TimeSyncACK, Bit2:StatusACK, Bit3:UserVerifyACK, Bit4:TrackInACK, Bit5:TrackOutACK, Bit6:Stop, Bit7:Start |

---

## 2. 流程連續資料區 (Data Blocks)

### 2.1 人員驗證專區 (位址 40010 - 40099)
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40010 | **40w** | PLC→EAP | **[R]** Barcode (支援至 80 字元) |
| 40050 | **20w** | EAP→PLC | **[W]** Result Message (驗證結果文字) |

### 2.2 進站報工專區 (位址 40100 - 40199)
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40100 | **10w** | PLC→EAP | **[R]** Cassette ID (進站載具編號) |
| 40110 | **10w** | PLC→EAP | **[R]** Work Order (進站工單號碼) |
| 40120 | **20w** | EAP→PLC | **[W]** Recipe ID (支援至 40 字元程式名) |
| 40140 | **20w** | EAP→PLC | **[W]** Result Message (進站處理結果) |

### 2.3 出站報工專區 (位址 40200 - 40299)
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40200 | **10w** | PLC→EAP | **[R]** Cassette ID (出站載具編號) |
| 40210 | **10w** | PLC→EAP | **[R]** Work Order (出站工單號碼) |
| 40220 | 1w | PLC→EAP | **[R]** Output Qty (產出數量) |
| 40230 | **20w** | EAP→PLC | **[W]** Result Message (出站處理結果) |

### 2.4 背景監控與共用專區 (位址 40300 - 40399)
| 位址 | 長度 | 方向 | 說明 | 觸發模式 |
| :--- | :--- | :--- | :--- | :--- |
| 40300 | 1w | PLC→EAP | **[R]** Alarm Trigger | ROLLING |
| 40301 | 1w | PLC→EAP | **[R]** Alarm Code | - |
| 40302 | 1w | EAP→PLC | **[W]** Alarm ACK | - |
| 40310 | 1w | PLC→EAP | **[R]** Data Trigger | ROLLING |
| 40311 | **20w** | PLC→EAP | **[R]** Process Data | - |
| 40331 | 1w | EAP→PLC | **[W]** Data ACK | - |
| 40350 | 7w | EAP→PLC | **[W]** System Time (YYYYMMDDHHmmss) | BIT 1 |

---

## 3. 交握邏輯摘要
*   **BIT 模式**: 40001 中的 Bit (3, 4, 5) 設為 1 -> EAP 完成後 40002 對應 Bit 設為 1 -> PLC 清 0 -> EAP 清 0。
*   **ROLLING 模式**: 40300 / 40310 數值變動觸發 -> EAP 處理後寫回相同值至 ACK 位址。

---
**版本**: V1.4 | **日期**: 2026-03-30
