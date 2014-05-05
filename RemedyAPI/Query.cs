using BMC.ARSystem;

namespace RemedyAPI {
    class Query {
        public string queryString;
        public EntryFieldValueList results;

        public Query( string query ) {
            this.queryString = query;
        }
    }
}
