# Modbus TCP 通訊規格書 (EAP V1.0.3 標準) - 版本 V1.7

本規格書嚴格對照 V1.0.3 JSON 結構，並針對長字串（80碼 Barcode, 60碼材料/治具 ID）進行長度優化與流程連續位址規劃。

## 1. 狀態與控制位元組 (Packed Bits)
| 位址 | 名稱 | 方向 | Bit 定義 (位元交握) |
| :--- | :--- | :--- | :--- |
| **40001** | **PLC Status Word** | PLC→EAP | Bit0:HBT, Bit1:TimeSync, Bit2:StatusQuery, Bit3:UserVerify, Bit4:TrackIn, Bit5:TrackOut, Bit6:Ready, Bit7:Alarm |
| **40002** | **EAP Control Word** | EAP→PLC | Bit1:TimeSyncACK, Bit2:StatusACK, Bit3:UserVerifyACK, Bit4:TrackInACK, Bit5:TrackOutACK, Bit6:Stop, Bit7:Start |

---

## 2. 流程資料專區 (長字串擴展版)

### 2.1 人員驗證專區 (UserVerify)
| 位址 | 長度 (Word) | 方向 | 說明 (1w = 2 chars) |
| :--- | :--- | :--- | :--- |
| 40010 | 10w | PLC→EAP | **[R]** UserID (工號，可達 20 碼) |
| 40020 | 40w | PLC→EAP | **[R]** Barcode (人員長代碼，可達 80 碼) |
| 40060 | 20w | EAP→PLC | **[W]** ResultMsg (驗證結果回覆) |

### 2.2 進站報工專區 (Track-In)
| 位址 | 長度 (Word) | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40100 | 10w | PLC→EAP | **[R]** UserID (報工工號) |
| 40110 | 10w | PLC→EAP | **[R]** CassetteID (載具編號，支援 14 碼) |
| 40120 | 10w | PLC→EAP | **[R]** WorkOrder (工單號碼，支援 12 碼) |
| 40130 | 30w | PLC→EAP | **[R]** MaterialID (材料ID，可達 60 碼) |
| 40160 | 30w | PLC→EAP | **[R]** ToolingID (治具ID，可達 60 碼) |
| 40190 | 10w | PLC→EAP | **[R]** PanelID (板號，支援 16 碼) |
| 40200 | 1w | PLC→EAP | **[R]** Qty (進站數量) |
| 40210 | 20w | EAP→PLC | **[W]** RecipeID (MES 指派程式，支援 40 碼) |
| 40230 | 20w | EAP→PLC | **[W]** ResultMsg (進站結果回覆) |

### 2.3 出站報工專區 (Track-Out)
| 位址 | 長度 (Word) | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40300 | 10w | PLC→EAP | **[R]** UserID (報工工號) |
| 40310 | 10w | PLC→EAP | **[R]** CassetteID (載具編號) |
| 40320 | 10w | PLC→EAP | **[R]** WorkOrder (工單號碼) |
| 40330 | 1w | PLC→EAP | **[R]** Qty (產出數量) |
| 40340 | 10w | PLC→EAP | **[R]** Result (PASS/FAIL) |
| 40350 | 10w | PLC→EAP | **[R]** NGCode (不良代碼) |
| 40360 | 20w | EAP→PLC | **[W]** ResultMsg (出站結果回覆) |

### 2.4 狀態、警報與數據 (Rolling Trigger)
| 觸發位址 | ACK位址 | 資料位址 | 長度 | 說明 |
| :--- | :--- | :--- | :--- | :--- |
| 40003 | 41003 | 40500 | 1w | NewStatus |
| - | - | 40501 | 1w | OldStatus |
| - | - | 40502 | 10w | ReasonCode |
| 40001 | 41001 | 40600 | 10w | AlarmCode |
| - | - | 40610 | 20w | AlarmText |
| 40002 | 41002 | 40700 | 10w | WorkOrder (數據關聯) |
| - | - | 40710 | 30w | DataPayload (數據內容，可達 60 碼) |

---
**版本**: V1.7 | **日期**: 2026-03-30
