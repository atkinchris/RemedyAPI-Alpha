using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;

namespace RemedyAPI {
    public class RemedyQuery {

        private string _server;
        private string _form;
        private string _username;
        private string _password;

        private List<string> _groups;
        private List<uint> _fields;

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

        public void AddGroup( string group ) {
            if ( group.IsNullOrBlank() ) {
                throw new ArgumentException( "Group name must not be blank." );
            }
            else if ( !Regex.IsMatch( group, @"^[a-zA-Z0-9\:\-\&]+$" ) ) {
                throw new ArgumentException( string.Format( "Group name contains invalid characers.", group ) );
            }
            _groups.Add( group );
        }

        public void AddGroups( string[] groups ) {
            foreach ( var group in groups ) {
                AddGroup( group );
            }
        }

        public void DeleteGroup( string group ) {
            if ( _groups.Contains( group ) ) {
                _groups.Remove( group );
            }
            else {
                throw new ArgumentException( string.Format( "Group name {0} does not exist.", group ) );
            }
        }

        public void DeleteGroups( string[] groups ) {
            foreach ( var group in groups ) {
                DeleteGroup( group );
            }
        }

        public void ClearGroups() {
            _groups.Clear();
        }

        public void AddField( uint field ) {
            _fields.Add( field );
        }

        public void AddFields( uint[] fields ) {
            foreach ( var field in fields ) {
                AddField( field );
            }
        }

        public void DeleteField( uint field ) {
            if ( _fields.Contains( field ) ) {
                _fields.Remove( field );
            }
            else {
                throw new ArgumentException( string.Format( "Field {0} does not exist.", field.ToString() ) );
            }
        }

        public void DeleteFields( uint[] fields ) {
            foreach ( var field in fields ) {
                DeleteField( field );
            }
        }

    }
}
