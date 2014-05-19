﻿using System;
using System.Collections.Generic;

namespace RemedyAPI_Example {

    public class FlotSeries {
        public string label;
        public List<List<object>> data;
        public string color;

        public FlotSeries( string label, Dictionary<DateTime, List<int>> data, int column, string color = "#D9EDF7") {
            this.label = label;
            this.color = color;
            this.data = new List<List<object>>();
            foreach ( var row in data ) {
                var timestamp = row.Key.ToJavascriptTime();
                var value = row.Value[ column - 1 ];
                this.data.Add( new List<object> { timestamp, value });
            }
        }
    }
}
