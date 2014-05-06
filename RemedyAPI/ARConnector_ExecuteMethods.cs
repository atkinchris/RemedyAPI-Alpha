
namespace RemedyAPI {
    public partial class ARConnector {

        /// <summary>
        /// Execute all queries against the Remedy server.
        /// </summary>
        public void ExecuteQueries() {
            server.ExecuteQuery( queries, groups, fields );
        }

        /// <summary>
        /// Execute a specific query against the Remedy server.
        /// </summary>
        /// <param name="title">Query title</param>
        public void ExecuteQuery( string title ) {
            var query = queries.GetQuery( title );
            server.ExecuteQuery( query, groups, fields );
        }
    }
}
