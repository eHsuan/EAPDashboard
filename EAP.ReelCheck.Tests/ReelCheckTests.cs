using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using EAP.ReelCheck.Engine;
using EAP.ReelCheck.Models;
using EAP.ReelCheck.Parsers;

namespace EAP.ReelCheck.Tests;

public class ReelCheckTests
{
    private static string Build2DBarcode(string matNo, string batchNo, string typeCode, string cipher)
    {
        // 索引 0: matNo
        // 索引 1, 2, 3: 空
        // 索引 4: batchNo
        // 索引 5: typeCode
        // 索引 6, 7: 空
        // 索引 8: cipher
        return matNo + "{" + "{" + "{" + "{" + batchNo + "{" + typeCode + "{" + "{" + "{" + cipher;
    }

    [Fact]
    public void Barcode2DParser_DecodeReelId_ShouldCorrectlyMapCharacters()
    {
        // Arrange
        string plainReelId = "REEL2026";
        string cipherWithPrefix = Barcode2DParser.EncodeReelId(plainReelId, 'R');

        // Act
        string decoded = Barcode2DParser.DecodeReelId(cipherWithPrefix);

        // Assert
        decoded.Should().Be(plainReelId);
    }

    [Fact]
    public void Barcode2DParser_ParseValid2D_ShouldExtractAllFields()
    {
        // Arrange
        string rawCipher = Barcode2DParser.EncodeReelId("REEL001");
        string barcode = Build2DBarcode("MAT-1001-CYN", "BATCH-888888", "88", rawCipher);

        // Act
        var result = Barcode2DParser.Parse(barcode);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Is2D.Should().BeTrue();
        result.MatNo.Should().Be("MAT-1001-CYN");
        result.TapingNo.Should().Be("BATCH-888888");
        result.TypeCode.Should().Be("88");
        result.DecodedReelId.Should().Be("REEL001");
    }

    [Fact]
    public void BarcodeFilter_NoiseFiltering_ShouldCorrectlyIdentifyInvalidBarcodes()
    {
        // 1. 純數字應過濾 (NG-9901)
        var res1 = BarcodeFilter.SanitizeAndValidate("1234567890", "MAT-1001-CYN", "BATCH-888888");
        res1.IsIgnored.Should().BeTrue();
        res1.ErrorCode.Should().Be("NG-9901");

        // 2. 長度小於 9 應過濾 (NG-9903)
        var res2 = BarcodeFilter.SanitizeAndValidate("SHORT", "MAT-1001-CYN", "BATCH-888888");
        res2.IsIgnored.Should().BeTrue();
        res2.ErrorCode.Should().Be("NG-9903");

        // 3. 硬體錯誤標籤 (NG-9801)
        var res3 = BarcodeFilter.SanitizeAndValidate("ERROR_CANNOT_READ", "MAT-1001-CYN", "BATCH-888888");
        res3.IsHardwareError.Should().BeTrue();
        res3.ErrorCode.Should().Be("NG-9801");

        // 4. 異常符號自動修復 (NG-9802)
        var res4 = BarcodeFilter.SanitizeAndValidate("VALIDBARCODE:999%", "MAT-1001-CYN", "BATCH-888888");
        res4.CleanedBarcode.Should().Be("VALIDBARCODE");
    }

    [Fact]
    public void ReelCheckEngine_HappyPath_ShouldPassAllVerifications()
    {
        // Arrange
        var engine = new ReelCheckEngine();
        var context = new WorkOrderContext
        {
            WONo = "WO-2026-00001",
            MatNo = "MAT-1001-CYN",
            BatchNo = "BATCH-888888",
            MachineNo = "EQ-01",
            OperatorId = "OP-007",
            MinMatNoScanCount = 1,
            MinTPNoScanCount = 1
        };

        var reels = new List<ReelItem>
        {
            new() { ReelId = "REEL001", Status = "00", TPMachNo = "EQ-01" },
            new() { ReelId = "REEL002", Status = "00", TPMachNo = "EQ-01" }
        };

        engine.Initialize(context, reels);

        // 1. 刷入大標籤料號
        var matResult = engine.ProcessBarcode("MAT-1001-CYN");
        matResult.IsSuccess.Should().BeTrue();
        matResult.BarcodeType.Should().Be(BarcodeKind.MatNo);

        // 2. 刷入大標籤批號
        var batchResult = engine.ProcessBarcode("BATCH-888888");
        batchResult.IsSuccess.Should().BeTrue();
        batchResult.BarcodeType.Should().Be(BarcodeKind.TapingNo);

        // 3. 刷入 2D Reel 條碼
        string cipher = Barcode2DParser.EncodeReelId("REEL001");
        string reelBarcode = Build2DBarcode("MAT-1001-CYN", "BATCH-888888", "88", cipher);

        var reelResult = engine.ProcessBarcode(reelBarcode);

        // Assert
        reelResult.IsSuccess.Should().BeTrue();
        reelResult.BarcodeType.Should().Be(BarcodeKind.Reel2D);
        reelResult.DecodedReelId.Should().Be("REEL001");
        reelResult.ActiveReel.Should().NotBeNull();
        reelResult.ActiveReel!.Status.Should().Be("11");
        reelResult.ActiveReel.ReelScanCount.Should().Be(1);
    }

    [Fact]
    public void ReelCheckEngine_WrongMaterial_ShouldTriggerBlockWorkOrder()
    {
        // Arrange: 混料情境 (料號不符)
        var engine = new ReelCheckEngine();
        var context = new WorkOrderContext
        {
            WONo = "WO-2026-00001",
            MatNo = "MAT-CORRECT-CYN",
            BatchNo = "BATCH-888888",
            MachineNo = "EQ-01"
        };

        var reels = new List<ReelItem>
        {
            new() { ReelId = "REEL001", Status = "00" }
        };

        engine.Initialize(context, reels);

        // 刷入錯誤料號之 2D 條碼
        string cipher = Barcode2DParser.EncodeReelId("REEL001");
        string wrongMatBarcode = Build2DBarcode("MAT-WRONG-CYN", "BATCH-888888", "88", cipher);

        // Act
        var result = engine.ProcessBarcode(wrongMatBarcode);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("NG-0106");
        result.ShouldBlockWorkOrder.Should().BeTrue();
        result.Disposition.Should().Be(CheckDisposition.BlockWorkOrder);
    }

    [Fact]
    public void ReelCheckEngine_UnboundReel_ShouldTriggerBlockWorkOrder()
    {
        // Arrange: 跨工單非法 Reel
        var engine = new ReelCheckEngine();
        var context = new WorkOrderContext
        {
            WONo = "WO-2026-00001",
            MatNo = "MAT-1001-CYN",
            BatchNo = "BATCH-888888",
            MachineNo = "EQ-01"
        };

        var reels = new List<ReelItem>
        {
            new() { ReelId = "REEL001", Status = "00" }
        };

        engine.Initialize(context, reels);

        // 刷入不屬於本工單的 REEL999
        string cipher = Barcode2DParser.EncodeReelId("REEL999");
        string alienReelBarcode = Build2DBarcode("MAT-1001-CYN", "BATCH-888888", "88", cipher);

        // Act
        var result = engine.ProcessBarcode(alienReelBarcode);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("NG-0301");
        result.ShouldBlockWorkOrder.Should().BeTrue();
    }

    [Fact]
    public void ReelCheckEngine_WrongMachineBinding_ShouldTriggerBlockWorkOrder()
    {
        // Arrange: 已綁定至其他機台 EQ-02，但攜至 EQ-01 掃描
        var engine = new ReelCheckEngine();
        var context = new WorkOrderContext
        {
            WONo = "WO-2026-00001",
            MatNo = "MAT-1001-CYN",
            BatchNo = "BATCH-888888",
            MachineNo = "EQ-01"
        };

        var reels = new List<ReelItem>
        {
            new() { ReelId = "REEL001", Status = "00", TPMachNo = "EQ-02" }
        };

        engine.Initialize(context, reels);

        string cipher = Barcode2DParser.EncodeReelId("REEL001");
        string barcode = Build2DBarcode("MAT-1001-CYN", "BATCH-888888", "88", cipher);

        // Act
        var result = engine.ProcessBarcode(barcode);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("NG-0109");
        result.ShouldBlockWorkOrder.Should().BeTrue();
    }

    [Fact]
    public async Task ReelCheckService_FullLifecycle_ShouldInitializeAndHandleScanResult()
    {
        // Arrange
        var mockMes = new Moq.Mock<EAP.ReelCheck.Communication.IReelCheckMesService>();
        mockMes.Setup(m => m.QueryWorkOrderInfoAsync("WO-01", "EQ-01", "OP-01", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new WorkOrderContext
            {
                WONo = "WO-01",
                MatNo = "MAT-1001-CYN",
                BatchNo = "BATCH-888888",
                MachineNo = "EQ-01",
                OperatorId = "OP-01"
            });

        mockMes.Setup(m => m.QueryReelListAsync("WO-01", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new List<ReelItem>
            {
                new() { ReelId = "REEL-999", Status = "00", TPMachNo = "EQ-01", MatNo = "MAT-1001-CYN", BatchNo = "BATCH-888888" }
            });

        var service = new EAP.ReelCheck.Service.ReelCheckService(mockMes.Object);
        var scanEvents = new List<ReelCheckResult>();
        service.OnScanResult += (s, e) => scanEvents.Add(e);

        // Act 1: 初始化工單
        var ctx = await service.InitializeWorkOrderAsync("WO-01", "EQ-01", "OP-01");
        service.IsInitialized.Should().BeTrue();
        ctx.WONo.Should().Be("WO-01");

        // Act 2: 掃描 1D 料號 (>= 9 碼)
        var r1 = await service.ScanAsync("MAT-1001-CYN");
        r1.IsSuccess.Should().BeTrue();
        r1.BarcodeType.Should().Be(BarcodeKind.MatNo);

        // Act 3: 掃描 2D Reel
        string cipher = Barcode2DParser.EncodeReelId("REEL-999");
        string barcode = Build2DBarcode("MAT-1001-CYN", "BATCH-888888", "88", cipher);
        var r2 = await service.ScanAsync(barcode);

        // Assert
        r2.IsSuccess.Should().BeTrue();
        r2.DecodedReelId.Should().Be("REEL-999");
        scanEvents.Count.Should().Be(2);
    }

    [Fact]
    public async Task ReelCheckService_BlockWorkOrder_ShouldTriggerOnReelBlockedAndCallMes()
    {
        // Arrange
        var mockMes = new Moq.Mock<EAP.ReelCheck.Communication.IReelCheckMesService>();
        mockMes.Setup(m => m.QueryWorkOrderInfoAsync("WO-01", "EQ-01", "OP-01", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new WorkOrderContext
            {
                WONo = "WO-01",
                MatNo = "MAT-1001-CYN",
                BatchNo = "BATCH-888888",
                MachineNo = "EQ-01",
                OperatorId = "OP-01"
            });

        mockMes.Setup(m => m.QueryReelListAsync("WO-01", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new List<ReelItem>
            {
                new() { ReelId = "REEL-LEGAL", Status = "00", TPMachNo = "EQ-01" }
            });

        var service = new EAP.ReelCheck.Service.ReelCheckService(mockMes.Object);
        ReelCheckResult? blockedResult = null;
        service.OnReelBlocked += (s, e) => blockedResult = e;

        await service.InitializeWorkOrderAsync("WO-01", "EQ-01", "OP-01");

        // Act: 掃入非本工單的 Reel
        string cipherIllegal = Barcode2DParser.EncodeReelId("REEL-ILLEGAL");
        string barcode = Build2DBarcode("MAT-1001-CYN", "BATCH-888888", "88", cipherIllegal);
        var scanRes = await service.ScanAsync(barcode);

        // Assert
        scanRes.IsSuccess.Should().BeFalse();
        scanRes.ShouldBlockWorkOrder.Should().BeTrue();
        blockedResult.Should().NotBeNull();
        blockedResult!.ErrorCode.Should().Be("NG-0301");

        // 驗證向 MES 發出鎖單請求
        mockMes.Verify(m => m.BlockWorkOrderAsync("WO-01", "NG-0301", Moq.It.IsAny<string>(), Moq.It.IsAny<System.Threading.CancellationToken>()), Moq.Times.Once);
    }

    [Fact]
    public void ReelCheckOptions_DefaultValues_ShouldBeSetCorrectly()
    {
        var opt = new EAP.ReelCheck.Configuration.ReelCheckOptions();
        opt.ServerUrl.Should().NotBeNullOrWhiteSpace();
        opt.TimeoutSeconds.Should().Be(30);
        opt.AutoReconnect.Should().BeTrue();
    }

    [Fact]
    public async Task ReelCheckService_MissingMaterialOrBatch_ShouldThrowInvalidOperationException()
    {
        // Arrange: MES 回傳空料號或空批號
        var mockMes = new Moq.Mock<EAP.ReelCheck.Communication.IReelCheckMesService>();
        mockMes.Setup(m => m.QueryWorkOrderInfoAsync("WO-EMPTY", "EQ-01", "OP-01", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new WorkOrderContext
            {
                WONo = "WO-EMPTY",
                MatNo = "", // 空料號
                BatchNo = "", // 空批號
                MachineNo = "EQ-01",
                OperatorId = "OP-01"
            });

        var service = new EAP.ReelCheck.Service.ReelCheckService(mockMes.Object);

        // Act & Assert
        var act = async () => await service.InitializeWorkOrderAsync("WO-EMPTY", "EQ-01", "OP-01");
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*缺少料號或批號資訊*");
        service.IsInitialized.Should().BeFalse();
    }

    [Fact]
    public async Task ReelCheckService_NullContext_ShouldThrowInvalidOperationException()
    {
        // Arrange: MES 回傳 null
        var mockMes = new Moq.Mock<EAP.ReelCheck.Communication.IReelCheckMesService>();
        mockMes.Setup(m => m.QueryWorkOrderInfoAsync("WO-NULL", "EQ-01", "OP-01", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync((WorkOrderContext)null!);

        var service = new EAP.ReelCheck.Service.ReelCheckService(mockMes.Object);

        // Act & Assert
        var act = async () => await service.InitializeWorkOrderAsync("WO-NULL", "EQ-01", "OP-01");
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Context 為空*");
        service.IsInitialized.Should().BeFalse();
    }

    [Fact]
    public async Task ReelCheckService_CrossWorkOrderReel_ShouldTriggerNG0401AndDualBlock()
    {
        // Arrange
        var mockMes = new Moq.Mock<EAP.ReelCheck.Communication.IReelCheckMesService>();
        mockMes.Setup(m => m.QueryWorkOrderInfoAsync("WO-CURRENT", "EQ-01", "OP-01", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new WorkOrderContext
            {
                WONo = "WO-CURRENT",
                MatNo = "MAT-1001-CYN",
                BatchNo = "BATCH-888888",
                MachineNo = "EQ-01",
                OperatorId = "OP-01"
            });

        mockMes.Setup(m => m.QueryReelListAsync("WO-CURRENT", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new List<ReelItem>
            {
                new() { ReelId = "REEL-LEGIT-001", Status = "00", TPMachNo = "EQ-01" }
            });

        // 模擬 MES 反查：REEL-OTHER-001 屬於其他工單 WO-OTHER-999
        mockMes.Setup(m => m.QueryOtherWorkOrderByBarcodeAsync("REEL-OTHER-001", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync("WO-OTHER-999");

        var service = new EAP.ReelCheck.Service.ReelCheckService(mockMes.Object);
        await service.InitializeWorkOrderAsync("WO-CURRENT", "EQ-01", "OP-01");

        // Act: 掃入屬於其他工單的 Reel
        string cipherOther = Barcode2DParser.EncodeReelId("REEL-OTHER-001");
        string barcode = Build2DBarcode("MAT-1001-CYN", "BATCH-888888", "88", cipherOther);
        var result = await service.ScanAsync(barcode);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("NG-0401");
        result.ProblemWoNo.Should().Be("WO-OTHER-999");
        result.Message.Should().Contain("WO-CURRENT").And.Contain("WO-OTHER-999");

        // 驗證雙向鎖單：同時鎖定當前工單與對方問題工單
        mockMes.Verify(m => m.BlockWorkOrderAsync("WO-CURRENT", "NG-0401", Moq.It.IsAny<string>(), Moq.It.IsAny<System.Threading.CancellationToken>()), Moq.Times.Once);
        mockMes.Verify(m => m.BlockWorkOrderAsync("WO-OTHER-999", "NG-0401B", Moq.It.IsAny<string>(), Moq.It.IsAny<System.Threading.CancellationToken>()), Moq.Times.Once);
    }

    [Fact]
    public void ReelCheckEngine_MainMaterial_ScanSubBatch_ShouldTriggerNG0103()
    {
        // Arrange
        var engine = new ReelCheckEngine();
        var context = new WorkOrderContext
        {
            WONo = "WO-01",
            MatNo = "MAT-MAIN",
            BatchNo = "BATCH-MAIN-001",
            SubMatNo = "MAT-SUB",
            SubBatchNo = "BATCH-SUB-999",
            IsMainMaterial = true,
            MachineNo = "EQ-01"
        };
        engine.Initialize(context, new List<ReelItem>());

        // Act: 掃入副料批號
        var result = engine.ProcessBarcode("BATCH-SUB-999");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("NG-0103");
        result.ShouldBlockWorkOrder.Should().BeTrue();
    }

    [Fact]
    public async Task ReelCheckService_FinishWorkOrder_ValidActiveReel_ShouldPassAndCallMes()
    {
        // Arrange
        var mockMes = new Mock<Communication.IReelCheckMesService>();
        mockMes.Setup(m => m.QueryWorkOrderInfoAsync("WO-001", "EQ-01", "OP-01", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new WorkOrderContext
            {
                WONo = "WO-001",
                MatNo = "MAT-1001-CYN",
                BatchNo = "BATCH-888888",
                MachineNo = "EQ-01",
                OperatorId = "OP-01",
                MinMatNoScanCount = 1,
                MinTPNoScanCount = 1
            });
        mockMes.Setup(m => m.QueryReelListAsync("WO-001", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new List<ReelItem>
            {
                new() { ReelId = "REEL-001", Status = "00", TPMachNo = "EQ-01", MatNo = "MAT-1001-CYN", BatchNo = "BATCH-888888" }
            });
        mockMes.Setup(m => m.CheckWorkOrderStatusAsync(Moq.It.IsAny<string>(), Moq.It.IsAny<string>(), Moq.It.IsAny<DateTime>(), Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(WorkOrderStatusCheckResult.Ok());

        var service = new Service.ReelCheckService(mockMes.Object);
        await service.InitializeWorkOrderAsync("WO-001", "EQ-01", "OP-01");

        // 1. 刷入 2D Reel 條碼完成上機 (設為 ActiveReel)
        string cipher = Barcode2DParser.EncodeReelId("REEL-001");
        string barcode = Build2DBarcode("MAT-1001-CYN", "BATCH-888888", "88", cipher);
        var r1 = await service.ScanAsync(barcode);
        r1.IsSuccess.Should().BeTrue();

        // 2. 刷入 1D 大標籤料號與批號累計防呆次數
        var r2 = await service.ScanAsync("MAT-1001-CYN");
        r2.IsSuccess.Should().BeTrue();
        r2.BarcodeType.Should().Be(BarcodeKind.MatNo);
        r2.ActiveReel.Should().NotBeNull();
        r2.ActiveReel!.MatNoScanCount.Should().Be(1);

        var r3 = await service.ScanAsync("BATCH-888888");
        r3.IsSuccess.Should().BeTrue();
        r3.BarcodeType.Should().Be(BarcodeKind.TapingNo);

        // Act: 執行工單結束作業 (WoFinish)
        var finishResult = await service.FinishWorkOrderAsync();

        // Assert
        finishResult.IsSuccess.Should().BeTrue();
        finishResult.ActiveReel.Should().NotBeNull();
        finishResult.ActiveReel!.IsCompleted.Should().BeTrue();
        mockMes.Verify(m => m.FinishWorkOrderAsync("WO-001", Moq.It.IsAny<ReelItem>(), "EQ-01", "OP-01", Moq.It.IsAny<System.Threading.CancellationToken>()), Moq.Times.Once);
    }

    [Fact]
    public async Task ReelCheckService_FinishWorkOrder_ActiveReelScanCountInsufficient_ShouldTriggerNG0111()
    {
        // Arrange
        var mockMes = new Mock<Communication.IReelCheckMesService>();
        mockMes.Setup(m => m.QueryWorkOrderInfoAsync("WO-001", "EQ-01", "OP-01", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new WorkOrderContext
            {
                WONo = "WO-001",
                MatNo = "MAT-1001-CYN",
                BatchNo = "BATCH-888888",
                MachineNo = "EQ-01",
                OperatorId = "OP-01",
                MinMatNoScanCount = 2, // 要求 2 次
                MinTPNoScanCount = 1
            });
        mockMes.Setup(m => m.QueryReelListAsync("WO-001", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new List<ReelItem>
            {
                new() { ReelId = "REEL-001", Status = "00", TPMachNo = "EQ-01", MatNo = "MAT-1001-CYN", BatchNo = "BATCH-888888" }
            });
        mockMes.Setup(m => m.CheckWorkOrderStatusAsync(Moq.It.IsAny<string>(), Moq.It.IsAny<string>(), Moq.It.IsAny<DateTime>(), Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(WorkOrderStatusCheckResult.Ok());

        var service = new Service.ReelCheckService(mockMes.Object);
        await service.InitializeWorkOrderAsync("WO-001", "EQ-01", "OP-01");

        // 1. 刷入 2D Reel 條碼完成上機 (設為 ActiveReel)
        string cipher = Barcode2DParser.EncodeReelId("REEL-001");
        string barcode = Build2DBarcode("MAT-1001-CYN", "BATCH-888888", "88", cipher);
        await service.ScanAsync(barcode);

        // 2. 只刷 1 次料號 (未達門檻 2 次) 與 1 次批號
        await service.ScanAsync("MAT-1001-CYN");
        await service.ScanAsync("BATCH-888888");

        // Act: 嘗試工單結束
        var finishResult = await service.FinishWorkOrderAsync();

        // Assert: 應阻斷並觸發 NG-0111
        finishResult.IsSuccess.Should().BeFalse();
        finishResult.ErrorCode.Should().Be("NG-0111");
        finishResult.ShouldBlockWorkOrder.Should().BeTrue();
        mockMes.Verify(m => m.BlockWorkOrderAsync("WO-001", "NG-0111", Moq.It.IsAny<string>(), Moq.It.IsAny<System.Threading.CancellationToken>()), Moq.Times.Once);
    }

    [Fact]
    public async Task ReelCheckService_DynamicWorkOrderStatusNG0203_ShouldBlockWorkOrder()
    {
        // Arrange
        var mockMes = new Mock<Communication.IReelCheckMesService>();
        mockMes.Setup(m => m.QueryWorkOrderInfoAsync("WO-001", "EQ-01", "OP-01", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new WorkOrderContext
            {
                WONo = "WO-001",
                MatNo = "MAT-001",
                BatchNo = "BATCH-001",
                ConfirmInTime = DateTime.Now.AddHours(-1)
            });
        mockMes.Setup(m => m.QueryReelListAsync("WO-001", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new List<ReelItem>());

        // 模擬工單已出站 (NG-0203)
        mockMes.Setup(m => m.CheckWorkOrderStatusAsync("WO-001", Moq.It.IsAny<string>(), Moq.It.IsAny<DateTime>(), Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(WorkOrderStatusCheckResult.Ng("NG-0203", "工單狀態變動(NG-0203), 工單出站狀態改變: 工單已出站"));

        var service = new Service.ReelCheckService(mockMes.Object);
        await service.InitializeWorkOrderAsync("WO-001", "EQ-01", "OP-01");

        // Act: 掃描任何條碼
        var result = await service.ScanAsync("MAT-001");

        // Assert: 應立即被動態狀態檢驗攔截並鎖單
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("NG-0203");
        result.ShouldBlockWorkOrder.Should().BeTrue();
        mockMes.Verify(m => m.BlockWorkOrderAsync("WO-001", "NG-0203", Moq.It.IsAny<string>(), Moq.It.IsAny<System.Threading.CancellationToken>()), Moq.Times.Once);
    }

    [Fact]
    public async Task ReelCheckService_Unknown1D_WhenBatchBelongsToOtherWO_ShouldTriggerNG0105DualBlock()
    {
        // Arrange
        var mockMes = new Mock<Communication.IReelCheckMesService>();
        mockMes.Setup(m => m.QueryWorkOrderInfoAsync("WO-CURR", "EQ-01", "OP-01", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new WorkOrderContext
            {
                WONo = "WO-CURR",
                MatNo = "MAT-CURR-001",
                BatchNo = "BATCH-CURR-001"
            });
        mockMes.Setup(m => m.QueryReelListAsync("WO-CURR", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new List<ReelItem>());
        mockMes.Setup(m => m.CheckWorkOrderStatusAsync(Moq.It.IsAny<string>(), Moq.It.IsAny<string>(), Moq.It.IsAny<DateTime>(), Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(WorkOrderStatusCheckResult.Ok());

        // 模擬 1D 批號屬於其他工單 WO-OTHER-888
        mockMes.Setup(m => m.QueryWorkOrderByBatchNoAsync("BATCH-OTHER-888", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync("WO-OTHER-888");

        var service = new Service.ReelCheckService(mockMes.Object);
        await service.InitializeWorkOrderAsync("WO-CURR", "EQ-01", "OP-01");

        // Act: 掃入非本工單之 1D 批號
        var result = await service.ScanAsync("BATCH-OTHER-888");

        // Assert: 應觸發 NG-0105 與雙向鎖單
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("NG-0105");
        result.ProblemWoNo.Should().Be("WO-OTHER-888");
        mockMes.Verify(m => m.BlockWorkOrderAsync("WO-CURR", "NG-0105", Moq.It.IsAny<string>(), Moq.It.IsAny<System.Threading.CancellationToken>()), Moq.Times.Once);
        mockMes.Verify(m => m.BlockWorkOrderAsync("WO-OTHER-888", "NG-0401B", Moq.It.IsAny<string>(), Moq.It.IsAny<System.Threading.CancellationToken>()), Moq.Times.Once);
    }

    [Fact]
    public async Task ReelCheckService_Unknown1D_WhenMaterialExists_ShouldTriggerNG0106Block()
    {
        // Arrange
        var mockMes = new Mock<Communication.IReelCheckMesService>();
        mockMes.Setup(m => m.QueryWorkOrderInfoAsync("WO-CURR", "EQ-01", "OP-01", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new WorkOrderContext
            {
                WONo = "WO-CURR",
                MatNo = "MAT-CURR-001",
                BatchNo = "BATCH-CURR-001"
            });
        mockMes.Setup(m => m.QueryReelListAsync("WO-CURR", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(new List<ReelItem>());
        mockMes.Setup(m => m.CheckWorkOrderStatusAsync(Moq.It.IsAny<string>(), Moq.It.IsAny<string>(), Moq.It.IsAny<DateTime>(), Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(WorkOrderStatusCheckResult.Ok());

        mockMes.Setup(m => m.QueryWorkOrderByBatchNoAsync("MAT-OTHER-999", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync((string?)null);
        mockMes.Setup(m => m.IsMaterialExistsAsync("MAT-OTHER-999", Moq.It.IsAny<System.Threading.CancellationToken>()))
            .ReturnsAsync(true);

        var service = new Service.ReelCheckService(mockMes.Object);
        await service.InitializeWorkOrderAsync("WO-CURR", "EQ-01", "OP-01");

        // Act: 掃入非本工單之其他料號
        var result = await service.ScanAsync("MAT-OTHER-999");

        // Assert: 應觸發 NG-0106 並鎖單
        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("NG-0106");
        result.ShouldBlockWorkOrder.Should().BeTrue();
        mockMes.Verify(m => m.BlockWorkOrderAsync("WO-CURR", "NG-0106", Moq.It.IsAny<string>(), Moq.It.IsAny<System.Threading.CancellationToken>()), Moq.Times.Once);
    }
}
