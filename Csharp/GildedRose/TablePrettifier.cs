using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace GildedRoseKata;

public static class TablePrettifier
{
    public static string TabulateItems<T>(IEnumerable<T> items, int xCellPadding = 2) where T : class
    {
        var jsonDoc = JsonSerializer.SerializeToDocument(items);
        var jsonArray = jsonDoc.RootElement;

        List<string> headers = [];
        List<List<string>> rows = [];
        List<int> columnWidths = [];

        foreach (var jElement in jsonArray.EnumerateArray())
        {
            List<string> row = [];

            var propIdx = 0;
            foreach (var jProp in jElement.EnumerateObject())
            {
                if (rows.Count == 0)
                {
                    headers.Add(jProp.Name);
                    columnWidths.Add(jProp.Name.Length);
                }

                var val = jProp.Value.ToString();
                var valLen = val.Length;
                row.Add(val);

                if (valLen > columnWidths[propIdx])
                {
                    columnWidths[propIdx] = valLen;
                }

                propIdx++;
            }

            rows.Add(row);
        }

        var tblBuilder = new System.Text.StringBuilder();
        // RC: Use blanks for the horizonatal line to substitute with padded dashes
        var hozRule = CreateTableLine(Enumerable.Range(0, rows[0].Count).Select(_ => string.Empty).ToList(),
            columnWidths, '+', xCellPadding, false, '-');

        tblBuilder.AppendLine(hozRule);
        // RC: Header Row
        tblBuilder.AppendLine(CreateTableLine(headers, columnWidths, '|', xCellPadding, true));
        tblBuilder.AppendLine(hozRule);

        foreach (var row in rows)
        {
            // RC: Content Rows
            tblBuilder.AppendLine(CreateTableLine(row, columnWidths, '|', xCellPadding, true));
        }

        tblBuilder.AppendLine(hozRule);

        return tblBuilder.ToString();
    }

    private static string CreateTableLine(List<string> items, List<int> columnWidths,
        char separator, int xPad, bool applySeparatorPad, char padChar = ' ')
    {
        // RC: If we're not padding the space around the cells add additional characters to left and right side
        // to pad the additional space
        var addedItemPadding = applySeparatorPad ? 0 : (xPad * 2);
        var sepSpacing = applySeparatorPad ? new string(' ', xPad) : string.Empty;
        var itemsPaddedByWidth = items.Select((item, idx) => item.PadRight(columnWidths[idx] + addedItemPadding, padChar));

        return string.Concat(string.Concat(string.Concat(separator, sepSpacing),
                        string.Join(string.Concat(sepSpacing, separator, sepSpacing), itemsPaddedByWidth),
                        string.Concat(sepSpacing, separator)));
    }
}