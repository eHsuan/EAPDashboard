using System;
using System.Collections.Generic;
using FluentAssertions;
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
}
