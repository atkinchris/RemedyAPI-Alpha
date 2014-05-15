using System;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace RemedyAPI {
    public class Server {

        private readonly BMC.ARSystem.Server _arserver = new BMC.ARSystem.Server();
        private readonly ObjectCache cache = MemoryCache.Default;

        private string _serverName;
        public string serverName {
            get {
                return _serverName;
            }
            set {
                if ( String.IsNullOrWhiteSpace( value ) ) {
                    throw new ArgumentException( "Server name must not be blank." );
                }
                if ( !Regex.IsMatch( value, @"^[a-zA-Z0-9-]+$" ) ) {
                    throw new ArgumentException( "Server name contains invalid characers." );
                }
                _serverName = value.ToLower();
            }
        }
        private string _formName;
        public string formName {
            get {
                return _formName;
            }
            set {
                if ( String.IsNullOrWhiteSpace( value ) ) {
                    throw new ArgumentException( "Form name must not be blank." );
                }
                if ( !Regex.IsMatch( value, @"^[a-zA-Z0-9: ]+$" ) ) {
                    throw new ArgumentException( "Form name contains invalid characers." );
                }
                _formName = value;
            }
        }
        private string _username;
        public string username {
            get {
                return _username;
            }
            set {
                if ( String.IsNullOrWhiteSpace( value ) ) {
                    throw new ArgumentException( "Username must not be blank." );
                }
                if ( !Regex.IsMatch( value, @"^[a-zA-Z0-9\-\.\']+$" ) ) {
                    throw new ArgumentException( "Server name contains invalid characers." );
                }
                _username = value;
            }
        }
        private string _password;
        public string password {
            set {
                if ( String.IsNullOrWhiteSpace( value ) ) {
                    throw new ArgumentException( "Password must not be blank." );
                }
                _password = value;
            }
        }
        private uint _maxRecords;
        public int maxRecords {
            get {
                return Convert.ToInt32( _maxRecords );
            }
            set {
                if ( value > 500 ) value = 500;
                _maxRecords = Convert.ToUInt32( value );
            }
        }
        private double _cacheTime;
        public int cacheTime {
            get {
                return Convert.ToInt32( _cacheTime );
            }
            set {
                if ( value < 15 ) value = 15;
                _cacheTime = Convert.ToDouble( value );
            }
        }
        private int[] fields {
            get {
                return Enum.GetValues( typeof( FieldId ) ).Cast<int>().ToArray();
            }
        }

        /// <summary>
        /// Creates a new ARSystem Server with credentials and server details.
        /// </summary>
        /// <param name="username">Username to use for connection</param>
        /// <param name="password">Password to use for connection</param>
        /// <param name="server">ARSystem Server name</param>
        /// <param name="form">Form name</param>
        /// <param name="maxRecords">Maximum number of records to return in queries</param>
        /// <param name="cacheTime">Length of time to cache results for</param>
        public Server( string username, string password, string server = "a-rrm-ars-p", string form = "HPD:Help Desk", int maxRecords = 500, int cacheTime = 60 ) {
            this.username = username;
            this.password = password;
            serverName = server;
            formName = form;
            this.maxRecords = maxRecords;
            this.cacheTime = cacheTime;
        }

        private void Login() {
            _arserver.Login( _serverName, _username, _password );
        }
        private void Logout() {
            _arserver.Logout();
        }

        /// <summary>
        /// Execute a single Query against the server.
        /// </summary>
        /// <param name="query">Query object to execute</param>
        public void ExecuteQuery( Query query ) {
            Login();
            RunQuery( query );
            Logout();
        }
        /// <summary>
        /// Execute multiple queries against the server in a single session.
        /// </summary>
        /// <param name="queries">Queries object containing queries</param>
        public void ExecuteQuery( Queries queries ) {
            Login();
            foreach ( var query in queries.Values ) {
                RunQuery( query );
            }
            Logout();
        }

        private void RunQuery( Query query ) {
            string queryString = query.ToString();
            var results = cache[queryString] as Results;

            if ( results == null ) {
                var efvl = _arserver.GetListEntryWithFields( _formName, queryString, fields, 0, _maxRecords );
                results = new Results( efvl );
                cache.Set( queryString, results, DateTime.Now.AddSeconds( _cacheTime ) );
            }

            query.results = results;
        }
    }
}
