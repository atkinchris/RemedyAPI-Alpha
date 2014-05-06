using System.Collections.Generic;

namespace RemedyAPI {
    internal class Query {
        private string _queryString;
        private List<Result> _results;
        public List<Result> results{
            get{
                return this._results;
            }
            set{
                this._results = value;
            }
        }

        public Query( string query ) {
            this._queryString = query;
        }

        public override string ToString() {
            return _queryString;
        }
    }
}
