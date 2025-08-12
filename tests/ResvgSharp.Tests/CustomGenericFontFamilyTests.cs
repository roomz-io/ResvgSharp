using System.IO;
using Xunit;

namespace ResvgSharp.Tests;

public class CustomGenericFontFamilyTests
{
    private const string TextSvgGenericFontFamily =
        """
        <svg width="200" height="100" xmlns="http://www.w3.org/2000/svg">
            <text x="10" y="50" font-family="sans-serif" font-size="24" fill="black">Hello World</text>
        </svg>
        """;

    private const string TextSvgInterFontFamily =
        """
        <svg width="200" height="100" xmlns="http://www.w3.org/2000/svg">
            <text x="10" y="50" font-family="Inter" font-size="24" fill="black">Hello World</text>
        </svg>
        """;

    private static readonly string OutputDir = Path.Combine("TestOutput");

    // Using Bold version to easily distinguish the output
    private static readonly string InterFontPath = Path.Combine("TestAssets", "fonts", "Inter-Bold.ttf");

    public CustomGenericFontFamilyTests()
    {
        if (!File.Exists(InterFontPath))
        {
            Assert.Fail("Font file not found: " + InterFontPath);
        }
    }

    static CustomGenericFontFamilyTests()
    {
        Directory.CreateDirectory(OutputDir);
    }

    private void SavePngOutput(byte[] pngBytes, string filename)
    {
        var outputPath = Path.Combine(OutputDir, filename);
        File.WriteAllBytes(outputPath, pngBytes);
    }

    [Fact]
    public void RenderToPng_WithGenericFontFamily_RendersDefaultSystemFont()
    {
        var fontData = File.ReadAllBytes(InterFontPath);
        var options = new ResvgOptions
        {
            UseFonts = [fontData],
        };

        var pngBytes = Resvg.RenderToPng(TextSvgGenericFontFamily, options);

        Assert.NotNull(pngBytes);
        Assert.True(pngBytes.Length > 0);

        SavePngOutput(pngBytes, "system-sans-serif-font-family.png");
    }

    [Fact]
    public void RenderToPng_WithGenericFontFamily_RendersSpecifiedFont()
    {
        var fontData = File.ReadAllBytes(InterFontPath);
        var options = new ResvgOptions
        {
            UseFonts = [fontData],
            SansSerifFamily = "Inter"
        };

        var pngBytes = Resvg.RenderToPng(TextSvgGenericFontFamily, options);

        Assert.NotNull(pngBytes);
        Assert.True(pngBytes.Length > 0);

        SavePngOutput(pngBytes, "custom-sans-serif-font-family.png");
    }

    [Fact]
    public void RenderToPng_WithGenericFontFamilyOrSpecificFont_RendersEqualImages()
    {
        var fontData = File.ReadAllBytes(InterFontPath);

        var optionsGeneric = new ResvgOptions
        {
            UseFonts = [fontData],
            SansSerifFamily = "Inter"
        };
        var pngBytesGeneric = Resvg.RenderToPng(TextSvgGenericFontFamily, optionsGeneric);

        Assert.NotNull(pngBytesGeneric);
        Assert.True(pngBytesGeneric.Length > 0);

        var optionsInter = new ResvgOptions
        {
            UseFonts = [fontData]
        };
        var pngBytesInter = Resvg.RenderToPng(TextSvgInterFontFamily, optionsInter);

        Assert.NotNull(pngBytesInter);
        Assert.True(pngBytesInter.Length > 0);

        Assert.Equal(pngBytesGeneric, pngBytesInter);
    }
}