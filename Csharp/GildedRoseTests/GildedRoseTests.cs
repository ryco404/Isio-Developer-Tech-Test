using Xunit;
using System.Collections.Generic;
using GildedRoseKata;
using static GildedRoseKata.SpecialItemNames;

namespace GildedRoseTests;

public class GildedRoseTests
{
    [Fact]
    public void UpdateQuality_QualityShouldNeverDropBelowZero()
    {
        // Arrange
        const int SellIn = 10;
        const int Quality = 5;

        IList<Item> items = GetTestItems("Test", SellIn, Quality);
        GildedRose app = new(items);

        // Act
        for (var i = 0; i < SellIn; ++i)
        {
            app.UpdateQuality();
        }

        // Assert
        Assert.Equal(0, items[0].Quality);
        Assert.Equal(0, items[0].SellIn);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(-1)]
    public void UpdateQuality_NormalItemQualityShouldDecreaseByTwoAfterSellByDate(int sellIn)
    {
        // Arrange
        const int Quality = 5;

        IList<Item> items = GetTestItems("Test", sellIn, Quality);
        GildedRose app = new(items);

        // Act
        app.UpdateQuality();

        // Assert
        var expectedDiff = sellIn < 0 ? 2 : 1;
        Assert.Equal(expectedDiff, Quality - items[0].Quality);
    }

    [Fact]
    public void UpdateQuality_SulfurasShouldNeverSellOrDegradeInQuality()
    {
        // Arrange
        const int Quality = 1;
        const int SellIn = 5;

        IList<Item> items = GetTestItems(Sulfuras, SellIn, Quality);
        GildedRose app = new(items);

        // Act
        app.UpdateQuality();

        // Assert
        Assert.Equal(Quality, items[0].Quality);
        Assert.Equal(SellIn, items[0].SellIn);
    }

    [Theory]
    [InlineData(Sulfuras)]
    [InlineData(BackstagePasses)]
    [InlineData(AgedBrie)]
    [InlineData(Conjured)]
    [InlineData("Miscellaneous")]
    public void UpdateQuality_OnlyAgedBrieShouldImproveInQualityAfterSellbyDateHasPassed(string itemName)
    {
        // Arrange
        const int Quality = 10;

        IList<Item> items = GetTestItems(itemName, 0, Quality);
        GildedRose app = new(items);

        // Act
        app.UpdateQuality();

        // Assert
        var expectedDiff = System.Math.Sign(itemName == AgedBrie ? 1 : -1);
        if (itemName == Sulfuras)
            expectedDiff = 0;

        Assert.Equal(expectedDiff, System.Math.Sign(items[0].Quality - Quality));
    }

    [Fact]
    public void UpdateQuality_BackstagePassesShouldIncreaseInQualityAsSellByDateApproachesThenZero()
    {
        // Arrange
        const int StartDay = 15;
        const int StartQuality = 10;
        IList<Item> items = GetTestItems(BackstagePasses, StartDay, StartQuality);
        var qualityChangeStops = new List<(int sellIn, int qualityChange)>();
        var prevChange = 1;

        GildedRose app = new(items);

        // Act
        for (var i = StartDay; i >= 0; --i)
        {
            var prevSellIn = items[0].SellIn;

            app.UpdateQuality();
            var change = items[0].Quality - StartQuality;

            if (prevChange != change)
            {
                qualityChangeStops.Add((prevSellIn, change));
                prevChange = change;
            }

            items[0].Quality = StartQuality;
        }

        // Assert
        int[] expectedSellInStops = [10, 5];
        Assert.Equal(expectedSellInStops.Length, qualityChangeStops.Count - 1);

        for (var i = 0; i < qualityChangeStops.Count - 1; ++i)
        {
            Assert.Equal(expectedSellInStops[i], qualityChangeStops[i].sellIn);
            Assert.Equal(i + 2, qualityChangeStops[i].qualityChange);
        }

        Assert.Equal(0, qualityChangeStops[^1].sellIn);
        Assert.Equal(-StartQuality, qualityChangeStops[^1].qualityChange);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(-1)]
    public void UpdateQuality_ConjuredItemQualityShouldDecreaseByTwiceAsFastAsNormalItems(int sellIn)
    {
        // Arrange
        const int Quality = 5;

        IList<Item> items = GetTestItems(Conjured, sellIn, Quality);
        GildedRose app = new(items);

        // Act
        app.UpdateQuality();

        // Assert
        var expectedDiff = sellIn < 0 ? 4 : 2;
        Assert.Equal(expectedDiff, Quality - items[0].Quality);
    }

    private IList<Item> GetTestItems(string name, int sellIn, int quality)
    {
        return [ new Item {
            Name = name,
            SellIn = sellIn,
            Quality = quality
        }];
    }
}