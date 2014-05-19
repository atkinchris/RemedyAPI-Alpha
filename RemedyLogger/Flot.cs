using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RemedyLogger {

    public class FlotSeries {
        [JsonProperty( PropertyName = "label" )]
        public string Label;
        [JsonProperty( PropertyName = "data" )]
        public readonly List<List<object>> Data = new List<List<object>>();
        [JsonProperty( PropertyName = "color" )]
        public string Color;

        public FlotSeries( string label, Dictionary<DateTime, List<int>> data, int column, string color = "#D9EDF7") {
            Label = label;
            Color = color;
            foreach ( var row in data ) {
                var timestamp = row.Key.ToJavascriptTime();
                var value = row.Value[ column - 1 ];
                Data.Add( new List<object> { timestamp, value });
            }
        }
    }
}
