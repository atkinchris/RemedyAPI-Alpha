using System;
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
                if ( String.IsNullOrWhiteSpace(value) ) {
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

        public Server( string username, string password, string server = "a-rrm-ars-p", string form = "HPD:Help Desk", int maxRecords = 500, int cacheTime = 60 ) {
            this.username = username;
            this.password = password;
            serverName = server;
            formName = form;
            this.maxRecords = maxRecords;
            this.cacheTime = cacheTime;
        }

        public void Login() {
            _arserver.Login( _serverName, _username, _password );
        }
        public void Logout() {
            _arserver.Logout();
        }

        public void ExecuteQuery( Query query ) {
            Login();
            RunQuery( query );
            Logout();
        }
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
                var efvl = _arserver.GetListEntryWithFields( _formName, query.ToString(), query.fields.ToArray(), 0, _maxRecords );
                results = new Results( efvl );
                cache.Set( queryString, results, DateTime.Now.AddSeconds( _cacheTime ) );
            }

            query.results = results;
        }
    }
}
