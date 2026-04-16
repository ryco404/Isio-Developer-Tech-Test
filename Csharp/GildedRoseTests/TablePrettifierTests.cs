using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using GildedRoseKata;
using static System.StringSplitOptions;

namespace GildedRoseTests;

public class TablePrettifierTests
{
    record TableTest(string A, string B, string C);

    private const int TableContentStartIdx = 3;

    [Fact]
    public void TabulateItems_ShouldSizeColumnToFitLongestItem()
    {
        // Arrange
        const int LongestPossibleStringLength = 50;
        const int NumItems = 5;
        var baseItem = new TableTest(string.Empty, "Foo", "Bar");
        var items = new List<TableTest>();

        for (var i = 0; i < NumItems; ++i)
        {
            items.Add(baseItem with
            {
                A = new string('x', Random.Shared.Next(LongestPossibleStringLength + 1))
            });
        }

        var longestLength = items.Max(tt => tt.A.Length);

        // Act
        var listAsTableDisplay = TablePrettifier.TabulateItems(items, 0);
        var tableLines = listAsTableDisplay.Split('\n');
        var firstRow = tableLines[TableContentStartIdx];
        var columnStart = 1;
        var columnEnd = firstRow.IndexOf('|', 1);

        // Assert
        Assert.Equal(longestLength, columnEnd - columnStart);
    }

    [Fact]
    public void TabulateItems_ShouldContainCorrectNumberOfRowsAndColumns()
    {
        // Arrange
        const int NumItems = 5;
        const int NumFields = 3;
        var items = new List<TableTest>();

        for (var i = 0; i < NumItems; ++i)
        {
            items.Add(new TableTest("Ryan", "C", "Web Developer"));
        }

        // Act
        var listAsTableDisplay = TablePrettifier.TabulateItems(items);
        var tableLines = listAsTableDisplay.Split('\n', RemoveEmptyEntries);
        // RC: Ignore last row as that's not a content row, rather the line at the end of the table
        var itemRows = tableLines[TableContentStartIdx..^1];
        var itemColumns = itemRows[0].Split('|', RemoveEmptyEntries);

        // Assert
        Assert.Equal(NumItems, itemRows.Length);
        Assert.Equal(NumFields, itemColumns.Length);
    }

    [Fact]
    public void TabulateItems_CellsShouldIncludeCorrectHorizontallyPadding()
    {
        // Arrange
        const int HozPadding = 4;
        var items = new List<TableTest>
        {
            new TableTest("Ryan", "C", "Web Developer")
        };

        // Act
        var listAsTableDisplay = TablePrettifier.TabulateItems(items, HozPadding);
        var firstRow = listAsTableDisplay.Split('\n', RemoveEmptyEntries)[TableContentStartIdx];
        var firstCell = firstRow.Split('|', RemoveEmptyEntries)[0];
        var expectedSpacing = new string(' ', HozPadding);

        // Assert
        Assert.StartsWith(expectedSpacing, firstCell);
        Assert.EndsWith(expectedSpacing, firstCell);
    }
}