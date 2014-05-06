
namespace RemedyAPI {
    public partial class ARConnector {
        private Queries queries = new Queries();

        public void AddQuery( string title, string query ) {
            queries.Add( title, query );
        }

        public void RemoveQuery( string title ) {
            queries.Remove( title );
        }
    }
}
