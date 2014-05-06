
namespace RemedyAPI {
    public class ARConnector {
        private Server server;
        private Groups groups = new Groups();
        private Fields fields = new Fields();
        private Queries queries = new Queries();

        /// <summary>
        /// RemedyQuery Constructor - called on creation of a new RemedyQuery object, and sets connection properties.
        /// Server and Form are set from default config, but can be changed with respective set methods.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="server"></param>
        /// <param name="form"></param>
        /// <param name="maxRecords"></param>
        public ARConnector( string username, string password, string server = "a-rrm-ars-p", string form = "HPD:Help Desk", int maxRecords = 500 ) {
            this.server = new Server() {
                username = username,
                password = password,
                serverName = server,
                formName = form,
                maxRecords = maxRecords
            };
        }

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
