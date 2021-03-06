﻿using System.Collections.Generic;
using ExtensionMinder;
using Vita.Contracts;

namespace Vita.Predictor.SpreadSheets
{
    public class LocalitiesSpreadsheet : SpreadSheetBase
    {
        private readonly List<Locality> _localities = new List<Locality>();

        public LocalitiesSpreadsheet()
        {
            var t = typeof(LocalitiesSpreadsheet).Namespace;
            LoadExcelSheet($"{t}.localities.xlsx");
        }

        public IEnumerable<Locality> LoadData()
        {
            var sheet = Workbook.Worksheets["suburbs"];
            for (var row = 2; row < sheet.Range.RowCount; row++)
            {
                var local = new Locality
                {
                    Postcode = sheet.Cells["A" + row].Text,
                    Suburb = sheet.Cells["B" + row].Text,
                    AustralianState = sheet.Cells["C" + row].Text.ToEnum<AustralianState>(),
                    Latitude = sheet.Cells["D" + row].Text.ToNullDouble(),
                    Longitude = sheet.Cells["E" + row].Text.ToNullDouble()
                };
                _localities.Add(local);
            }

            return _localities;
        }
    }
}