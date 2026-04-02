# Modbus TCP 通訊規格書 (EAP V1.0.3 標準) - 版本 V1.9

本規格書整合了 Bit 交握、Word ACK 與 Rolling Counter 三種模式，完美對應 V1.0.3 規範中所有主動與被動通訊需求。

## 1. 觸發與交握控制區

### 1.1 位元控制區 (Bit-Mapped)
| 位址 | 名稱 | 方向 | Bit 定義 |
| :--- | :--- | :--- | :--- |
| **40001** | **PLC Status Word** | PLC→EAP | Bit0:HBT, Bit1:TimeSync, Bit2:StatusQuery, Bit3:UserVerify, Bit4:TrackIn, Bit5:TrackOut |
| **40002** | **EAP Control Word** | EAP→PLC | Bit6:RemoteStop, Bit7:RemoteStart |

### 1.2 計數器交握區 (Rolling Counter)
| 觸發位址 | ACK 位址 | 名稱 | 說明 |
| :--- | :--- | :--- | :--- |
| 40003 | 41003 | Alarm Trigger | 警報上報觸發 |
| 40004 | 41004 | Data Trigger | 製程數據上報觸發 |
| 40005 | 41005 | Status Trigger | 機台狀態變更觸發 |

---

## 2. 流程資料專區

### 2.1 人員驗證專區 (Bit 3 觸發)
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40010 | 10w | PLC→EAP | UserID |
| 40020 | 40w | PLC→EAP | Barcode (80碼) |
| 40060 | 20w | EAP→PLC | ResultMsg |
| **40080** | **1w** | **EAP→PLC** | **ResultCode (0:Idle, 1:OK, 2:NG)** |

### 2.2 進站報工專區 (Bit 4 觸發)
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40100 | 10w | PLC→EAP | UserID / CassetteID / WorkOrder |
| 40130 | 30w | PLC→EAP | MaterialID (60碼) |
| 40160 | 30w | PLC→EAP | ToolingID (60碼) |
| 40190 | 10w | PLC→EAP | PanelID (20碼) |
| 40200 | 1w | PLC→EAP | Qty |
| 40210 | 20w | EAP→PLC | RecipeID (40碼) |
| 40230 | 20w | EAP→PLC | ResultMsg |
| **40250** | **1w** | **EAP→PLC** | **ResultCode (0:Idle, 1:OK, 2:NG)** |

### 2.3 出站報工專區 (Bit 5 觸發)
| 位址 | 長度 | 方向 | 說明 |
| :--- | :--- | :--- | :--- |
| 40300 | 10w | PLC→EAP | UserID / CassetteID / WorkOrder |
| 40330 | 1w | PLC→EAP | Qty |
| 40340 | 10w | PLC→EAP | Result (PASS/FAIL) / NGCode |
| 40360 | 20w | EAP→PLC | ResultMsg |
| **40380** | **1w** | **EAP→PLC** | **ResultCode (0:Idle, 1:OK, 2:NG)** |

### 2.4 背景上報專區 (Rolling 觸發)
| 數據起始位址 | 長度 | 說明 |
| :--- | :--- | :--- |
| 40400 | 10w | **[Alarm]** AlarmCode |
| 40410 | 20w | **[Alarm]** AlarmText |
| 40500 | 10w | **[Data]** WorkOrder |
| 40510 | 30w | **[Data]** DataPayload (60碼) |
| 40600 | 1w | **[Status]** NewStatus |
| 40601 | 1w | **[Status]** OldStatus |
| 40602 | 10w | **[Status]** ReasonCode |

---

## 3. 交握機制說明

### 3.1 Word ACK 模式 (進出站、人員)
*   PLC Set TriggerBit=1 -> EAP Done Set ResultCode=1/2 -> PLC Clear TriggerBit=0 -> EAP Clear ResultCode=0。

### 3.2 Rolling 模式 (警報、數據、狀態)
*   PLC Increment Trigger -> EAP Processed -> EAP Write ACK = Trigger。

---
**版本**: V1.9 | **日期**: 2026-03-30
