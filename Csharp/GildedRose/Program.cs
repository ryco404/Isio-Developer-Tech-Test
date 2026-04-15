using System;
using System.Collections.Generic;

namespace GildedRoseKata;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("*****************************");
        Console.WriteLine("*                           *");
        Console.WriteLine("*  Welcome to Gilded Rose!  *");
        Console.WriteLine("*                           *");
        Console.WriteLine("*****************************");

        IList<Item> items = new List<Item>
        {
            new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
            new Item {Name = SpecialItemNames.AgedBrie, SellIn = 2, Quality = 0},
            new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
            new Item {Name = SpecialItemNames.Sulfuras, SellIn = 0, Quality = 80},
            new Item {Name = SpecialItemNames.Sulfuras, SellIn = -1, Quality = 80},
            new Item
            {
                Name = SpecialItemNames.BackstagePasses,
                SellIn = 15,
                Quality = 20
            },
            new Item
            {
                Name = SpecialItemNames.BackstagePasses,
                SellIn = 10,
                Quality = 49
            },
            new Item
            {
                Name = SpecialItemNames.BackstagePasses,
                SellIn = 5,
                Quality = 49
            },
            new Item {Name = SpecialItemNames.Conjured, SellIn = 3, Quality = 6}
        };

        var app = new GildedRose(items);

        if (args.Length > 0 && int.TryParse(args[0], out int numDays))
        {
            numDays++;
        }
        else
        {
            numDays = 2;
        }

        for (var i = 0; i < numDays; i++)
        {
            Console.WriteLine("-------- day " + i + " --------");
            var itemsTableStr = app.ItemsToPrettifiedString();
            Console.WriteLine(itemsTableStr);

            app.UpdateQuality();
        }
    }
}