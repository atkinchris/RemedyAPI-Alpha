using BMC.ARSystem;
using System.Collections.Generic;

namespace RemedyAPI {
    class Query {
        public string queryString;
        private List<Result> _results;

        public Query( string query ) {
            this.queryString = query;
        }

        /// <summary>
        /// Parse a list of Result objects from the BMC type EntryFieldValueList.
        /// </summary>
        /// <param name="results">EntryFieldValueList of results</param>
        /// <returns>List of Result objects</returns>
        public void SetResults( EntryFieldValueList results ) {
            var parsedResults = new List<Result>();
            foreach ( var result in results ) {
                var parsedResult = new Result() {
                    entryID = result.EntryId
                };
                foreach ( var field in result.FieldValues.Keys ) {
                    parsedResult.fields.Add( field.ToString(), result.FieldValues[field] );
                }
            }
            this._results = parsedResults;
        }
    }
}
