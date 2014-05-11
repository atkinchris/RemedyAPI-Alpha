using System;
using System.Collections.Generic;
using System.Linq;

namespace RemedyAPI {
    public class Queries : Dictionary<string, Query> {

        /// <summary>
        /// Get a dictionary of query titles and their respective counts.
        /// </summary>
        /// <returns>Dictionary of titles and counts</returns>
        public Dictionary<string, int> GetResultsCount() {
            return this.ToDictionary( query => query.Key, query => query.Value.results.Count );
        }

        /// <summary>
        /// Get results as a string of their title and counts, each on a new line.
        /// </summary>
        /// <returns>String of results and their counts.</returns>
        public string GetResultsString() {
            var results = GetResultsCount();
            return String.Join( Environment.NewLine, results.Select( r => r.Key + ": " + r.Value ) );
        }
    }
}
