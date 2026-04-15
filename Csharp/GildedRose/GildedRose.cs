using System.Collections.Generic;

namespace GildedRoseKata;

public class GildedRose
{
    private IList<Item> _items;

    public GildedRose(IList<Item> items)
    {
        _items = items;
    }

    public void UpdateQuality()
    {
        for (var i = 0; i < _items.Count; i++)
        {
            var isAgedBrie = _items[i].Name == SpecialItemNames.AgedBrie;
            var isBackstagePass = _items[i].Name == SpecialItemNames.BackstagePasses;

            if (isAgedBrie || isBackstagePass)
            {
                var sellIn = _items[i].SellIn--;

                if (sellIn > 0 || isAgedBrie)
                {
                    var qualityIncrease = 1;

                    if (isBackstagePass)
                    {
                        if (sellIn < 11)
                        {
                            qualityIncrease += (sellIn < 6) ? 2 : 1;
                        }
                    }
                    else
                    {
                        qualityIncrease += (sellIn <= 0) ? 1 : 0;
                    }

                    while (qualityIncrease-- > 0 && _items[i].Quality < 50)
                    {
                        _items[i].Quality++;
                    }
                }
                else
                {
                    // RC: Backstage passes lose quality beyond sellby date
                    _items[i].Quality = 0;
                }
            }
            else
            {
                // RC: Not Sulfaras OR Brie OR Backstage Pass
                if (_items[i].Name != SpecialItemNames.Sulfuras)
                {
                    _items[i].SellIn--;
                    if (_items[i].Quality > 0)
                    {
                        var qualityDegradation = (_items[i].SellIn < 0) ? 2 : 1;
                        var degredationScaleFactor = _items[i].Name == SpecialItemNames.Conjured ? 2 : 1;

                        qualityDegradation *= degredationScaleFactor;

                        // RC: Quality should never dip below 0
                        _items[i].Quality -= System.Math.Min(qualityDegradation, _items[i].Quality);
                    }
                }
            }
        }
    }

    public string ItemsToPrettifiedString()
    {
        return TablePrettifier.TabulateItems(_items);
    }
}