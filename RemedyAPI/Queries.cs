using System;
using System.Collections.Generic;
using System.Linq;

namespace RemedyAPI {
    public class Queries : Dictionary<string, Query> {

        public Dictionary<string, int> GetResultsCount() {
            var output = new Dictionary<string, int>();
            foreach ( var query in this ) {
                output.Add( query.Key, query.Value.results.Count );
            }
            return output;
        }

        public string GetResultsString() {
            var results = GetResultsCount();
            return String.Join( Environment.NewLine, results.Select( r => r.Key + ": " + r.Value ) );
        }
    }
}
