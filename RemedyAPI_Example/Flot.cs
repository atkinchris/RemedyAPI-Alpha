using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemedyAPI_Example {

    public class FlotSeries {
        public string label { get; set; }
        public List<List<object>> data { get; set; }
        public string color { get; set; }
        public Series bars { get; set; }

        public FlotSeries( string label, Dictionary<DateTime, List<int>> data, int column ) {
            this.label = label;
            this.data = new List<List<object>>();
            foreach ( var row in data ) {
                var timestamp = row.Key.ToJavascriptTime();
                var value = row.Value[ column - 1 ];
                this.data.Add( new List<object> { timestamp, value });
            }
        }
    }

    public class Series {
        public bool show { get; set; }
        public int barWidth { get; set; }
        public int order { get; set; }
        public bool fill { get; set; }
        public int lineWidth { get; set; }
        public string fillColor { get; set; }
    }
}
