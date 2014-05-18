using System;
using System.Collections.Generic;

namespace RemedyAPI_Example {

    public class FlotSeries {
        public string Label;
        public List<List<object>> Data;
        public string Color;

        public FlotSeries( string label, Dictionary<DateTime, List<int>> data, int column, string colour = "#D9EDF7") {
            Label = label;
            Color = colour;
            Data = new List<List<object>>();
            foreach ( var row in data ) {
                var timestamp = row.Key.ToJavascriptTime();
                var value = row.Value[ column - 1 ];
                Data.Add( new List<object> { timestamp, value });
            }
        }
    }
}
