using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace RemedyAPI {
    internal class Queries {

        private Dictionary<string, Query> _queries = new Dictionary<string, Query>();

        //public void Add( string title, string query ) {
        //    _queries.Add( title, new Query( query ) );
        //}

        public void Remove( string title ) {
            _queries.Remove( title );
        }

        public Query GetQuery( string title ) {
            return _queries[title];
        }

        public List<Query> GetQueries() {
            return this._queries.Values.ToList();
        }

        public Dictionary<string, int> GetResultsCount() {
            var output = new Dictionary<string, int>();
            foreach (var query in _queries) {
                output.Add( query.Key, query.Value.ToInt() );
            }
            return output;
        }

        public string GetResultsString() {
            var output = new StringBuilder();
            var results = GetResultsCount();

            foreach (var result in results) {
                output.AppendFormat( "{0}: {1}", result.Key, result.Value.ToString() );
                if (!result.Equals( results.Last() )) output.Append( Environment.NewLine );
            }

            return output.ToString();
        }
    }
}
