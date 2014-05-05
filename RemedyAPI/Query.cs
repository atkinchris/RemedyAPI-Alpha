using BMC.ARSystem;

namespace RemedyAPI {
    class Query {
        private string _query;
        private EntryFieldValueList _results;

        public Query( string query ) {
            this._query = query;
        }

        public void Execute( Server server, string form, EntryListFieldList fields, uint maxRecords, string additionalQuery ) {
            _results = null;
            string queryString;
            if ( !additionalQuery.IsNullOrBlank() ) {
                queryString = string.Format( "{0} AND ({1})", additionalQuery, _query );
            }
            else {
                queryString = _query;
            }
            _results = server.GetListEntryWithFields( form, queryString, fields, 0, maxRecords );
        }
    }
}
