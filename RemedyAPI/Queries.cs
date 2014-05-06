using System.Collections.Generic;
using System.Linq;

namespace RemedyAPI {
    internal class Queries {

        private Dictionary<string, Query> _queries = new Dictionary<string, Query>();

        public void Add( string title, string query ) {
            _queries.Add( title, new Query( query ) );
        }

        public void Remove( string title ) {
            _queries.Remove( title );
        }

        public Query GetQuery( string title ) {
            return _queries[title];
        }

        public List<Query> GetQueries() {
            return this._queries.Values.ToList();
        }
    }
}
