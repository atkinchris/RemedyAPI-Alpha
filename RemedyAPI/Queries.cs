using System;
using System.Collections.Generic;
using System.Linq;

namespace RemedyAPI {
    public class Queries : Dictionary<string, Query> {

        public Dictionary<string, int> GetResultsCount() {
            return this.ToDictionary( query => query.Key, query => query.Value.results.Count );
        }

        public string GetResultsString() {
            var results = GetResultsCount();
            return String.Join( Environment.NewLine, results.Select( r => r.Key + ": " + r.Value ) );
        }
    }
}
