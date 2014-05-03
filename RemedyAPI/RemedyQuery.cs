using System;
using System.Configuration;

namespace RemedyAPI {
    public class RemedyQuery {

        private string _server = ConfigurationSettings.AppSettings["Default Server"];
        private string _form = ConfigurationSettings.AppSettings["Default Form"];
        private string _username;
        private string _password;


        public RemedyQuery( string username, string password ) {
            SetUsername( username );
            SetPassword( password );
        }

        public void SetUsername( string username ) {
            if ( username.IsNullOrBlank() ) {
                throw new Exception( "Username must not be blank." );
            }
            this._username = username;
        }

        public void SetPassword( string password ) {
            if ( password.IsNullOrBlank() ) {
                throw new Exception( "Username must not be blank." );
            }
            this._password = password;
        }

    }
}
