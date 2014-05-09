
namespace RemedyAPI {    
    public partial class ARConnector {
        private Server server;

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

        public void SetCredentials( string username, string password ) {
            this.server.username = username;
            this.server.password = password;
        }
    }
}
