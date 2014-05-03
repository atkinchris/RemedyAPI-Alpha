using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace RemedyAPI {
    public class RemedyQuery {

        private string _server;
        private string _form;
        private string _username;
        private string _password;


        public RemedyQuery( string username, string password ) {
            SetUsername( username );
            SetPassword( password );
            SetServer( ConfigurationSettings.AppSettings["Default Server"] );
            SetForm( ConfigurationSettings.AppSettings["Default Form"] );
        }

        public void SetUsername( string username ) {
            if ( username.IsNullOrBlank() ) {
                throw new ArgumentException( "Username must not be blank." );
            }
            else if ( !Regex.IsMatch( username, @"^[a-zA-Z0-9\-\.\']+$" ) ) {
                throw new ArgumentException( "Server name contains invalid characers." );
            }
            this._username = username;
        }

        public void SetPassword( string password ) {
            if ( password.IsNullOrBlank() ) {
                throw new ArgumentException( "Password must not be blank." );
            }
            this._password = password;
        }

        public void SetServer( string server ) {
            if ( server.IsNullOrBlank() ) {
                throw new ArgumentException( "Server name must not be blank." );
            }
            else if ( !Regex.IsMatch( server, @"^[a-zA-Z0-9\-]+$" ) ) {
                throw new ArgumentException( "Server name contains invalid characers." );
            }
            this._server = server.ToLower();
        }

        public void SetForm( string form ) {
            if ( form.IsNullOrBlank() ) {
                throw new ArgumentException( "Form name must not be blank." );
            }
            else if ( !Regex.IsMatch( form, @"^[a-zA-Z0-9\:\-]+$" ) ) {
                throw new ArgumentException( "Form name contains invalid characers." );
            }
            this._server = form;
        }

    }
}
