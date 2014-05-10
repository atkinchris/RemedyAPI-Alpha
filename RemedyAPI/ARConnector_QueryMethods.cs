
namespace RemedyAPI {
    public partial class ARConnector {
        private Queries queries = new Queries();

        public void AddQuery( string title, string query, bool filterAlerts = true ) {
            //if ( filterAlerts ) {
            //    query = string.Format( "({0}) AND (\'{1}\' < \"{2}\")", query, "Service Type", "Infrastructure Restoration" );
            //}
            //else {
            //    query = string.Format( "({0})", query );
            //}
            //queries.Add( title, query );
        }

        public void RemoveQuery( string title ) {
            queries.Remove( title );
        }
    }
}
