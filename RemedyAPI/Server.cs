using System;
using System.Text.RegularExpressions;

namespace RemedyAPI {
    class Server {

        private BMC.ARSystem.Server _arserver = new BMC.ARSystem.Server();

        private string _serverName;
        public string serverName {
            set {
                if ( value.IsNullOrBlank() ) {
                    throw new ArgumentException( "Server name must not be blank." );
                }
                else if ( !Regex.IsMatch( value, @"^[a-zA-Z0-9-]+$" ) ) {
                    throw new ArgumentException( "Server name contains invalid characers." );
                }
                this._serverName = value.ToLower();
            }
        }
        private string _formName;
        public string formName {
            set {
                if ( value.IsNullOrBlank() ) {
                    throw new ArgumentException( "Form name must not be blank." );
                }
                else if ( !Regex.IsMatch( value, @"^[a-zA-Z0-9: ]+$" ) ) {
                    throw new ArgumentException( "Form name contains invalid characers." );
                }
                this._formName = value;
            }
        }
        private string _username;
        public string username {
            set {
                if ( value.IsNullOrBlank() ) {
                    throw new ArgumentException( "Username must not be blank." );
                }
                else if ( !Regex.IsMatch( value, @"^[a-zA-Z0-9\-\.\']+$" ) ) {
                    throw new ArgumentException( "Server name contains invalid characers." );
                }
                this._username = value;
            }
        }
        private string _password;
        public string password {
            set {
                if ( value.IsNullOrBlank() ) {
                    throw new ArgumentException( "Password must not be blank." );
                }
                this._password = value;
            }
        }
        private uint _maxRecords;
        public int maxRecords {
            set {
                _maxRecords = Convert.ToUInt32( value );
            }
        }

        public void ExecuteQuery( Query query, Groups groups, Fields fields ) {
            _arserver.Login( _serverName, _username, _password );
            RunQuery( query, groups, fields );
            _arserver.Logout();
        }

        public void ExecuteQuery( Queries queries, Groups groups, Fields fields ) {
            _arserver.Login( _serverName, _username, _password );
            foreach ( var query in queries.GetQueries() ) {
                RunQuery( query, groups, fields );
            }
            _arserver.Logout();
        }

        private void RunQuery( Query query, Groups groups, Fields fields ) {
            var queryString = string.Format( "{0} AND ({1})", groups.ToString(), query.ToString() );
            var results = _arserver.GetListEntryWithFields( _formName, queryString, fields.ToArray(), 0, _maxRecords );
            query.results = results.ToResultsList();
        }
    }
}
