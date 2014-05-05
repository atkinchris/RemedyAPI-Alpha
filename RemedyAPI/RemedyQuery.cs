using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;

namespace RemedyAPI {
    public class RemedyQuery {

        // Connection Properties
        private string _server;
        private string _form;
        private string _username;
        private string _password;

        // Filters
        private List<string> _groups = new List<string>();
        private List<uint> _fields = new List<uint>();

        /// <summary>
        /// RemedyQuery Constructor - called on creation of a new RemedyQuery object, and sets connection properties.
        /// Server and Form are set from default config, but can be changed with respective set methods.
        /// </summary>
        /// <param name="username">Username for connection</param>
        /// <param name="password">Password for connection</param>
        public RemedyQuery( string username, string password ) {
            SetUsername( username );
            SetPassword( password );
            SetServer( ConfigurationSettings.AppSettings["Default Server"] );
            SetForm( ConfigurationSettings.AppSettings["Default Form"] );
        }

        /// <summary>
        /// Sets the username for the connection.
        /// </summary>
        /// <param name="username">Username</param>
        public void SetUsername( string username ) {
            if ( username.IsNullOrBlank() ) {
                throw new ArgumentException( "Username must not be blank." );
            }
            else if ( !Regex.IsMatch( username, @"^[a-zA-Z0-9\-\.\']+$" ) ) {
                throw new ArgumentException( "Server name contains invalid characers." );
            }
            this._username = username;
        }

        /// <summary>
        /// Sets the password for the connection.
        /// </summary>
        /// <param name="password">Password</param>
        public void SetPassword( string password ) {
            if ( password.IsNullOrBlank() ) {
                throw new ArgumentException( "Password must not be blank." );
            }
            this._password = password;
        }

        /// <summary>
        /// Sets the AR Server to connect to.
        /// </summary>
        /// <param name="server">Server name</param>
        public void SetServer( string server ) {
            if ( server.IsNullOrBlank() ) {
                throw new ArgumentException( "Server name must not be blank." );
            }
            else if ( !Regex.IsMatch( server, @"^[a-zA-Z0-9\-]+$" ) ) {
                throw new ArgumentException( "Server name contains invalid characers." );
            }
            this._server = server.ToLower();
        }

        /// <summary>
        /// Set the Form to retrieve data from.
        /// </summary>
        /// <param name="form">Form name</param>
        public void SetForm( string form ) {
            if ( form.IsNullOrBlank() ) {
                throw new ArgumentException( "Form name must not be blank." );
            }
            else if ( !Regex.IsMatch( form, @"^[a-zA-Z0-9\:\-]+$" ) ) {
                throw new ArgumentException( "Form name contains invalid characers." );
            }
            this._form = form;
        }

        /// <summary>
        /// Add a single group name to the list of groups to filter by.
        /// </summary>
        /// <param name="group">Group name</param>
        public void AddGroup( string group ) {
            if ( group.IsNullOrBlank() ) {
                throw new ArgumentException( "Group name must not be blank." );
            }
            else if ( !Regex.IsMatch( group, @"^[a-zA-Z0-9\:\-\&]+$" ) ) {
                throw new ArgumentException( string.Format( "Group name contains invalid characers.", group ) );
            }
            _groups.Add( group );
        }

        /// <summary>
        /// Add multiple group names to the list of groups to filter by.
        /// </summary>
        /// <param name="groups">Array of group names</param>
        public void AddGroups( string[] groups ) {
            foreach ( var group in groups ) {
                AddGroup( group );
            }
        }

        /// <summary>
        /// Delete a single group from the list of groups to filter by.
        /// </summary>
        /// <param name="group">Group name</param>
        public void DeleteGroup( string group ) {
            if ( _groups.Contains( group ) ) {
                _groups.Remove( group );
            }
            else {
                throw new ArgumentException( string.Format( "Group name {0} does not exist.", group ) );
            }
        }

        /// <summary>
        /// Delete multiple groups from the list of groups to filter by.
        /// </summary>
        /// <param name="groups">Array of group names</param>
        public void DeleteGroups( string[] groups ) {
            foreach ( var group in groups ) {
                DeleteGroup( group );
            }
        }

        /// <summary>
        /// Clear the list of groups to filter by.
        /// </summary>
        public void ClearGroups() {
            _groups.Clear();
        }

        /// <summary>
        /// Add a single field to be included in the results.
        /// </summary>
        /// <param name="fieldID">Field ID</param>
        public void AddField( uint fieldID ) {
            _fields.Add( fieldID );
        }

        /// <summary>
        /// Add multiple fields to be included in the results.
        /// </summary>
        /// <param name="fieldIDs">Array of Field IDs</param>
        public void AddFields( uint[] fieldIDs ) {
            foreach ( var fieldID in fieldIDs ) {
                AddField( fieldID );
            }
        }

        /// <summary>
        /// Delete a single field from the list to be returned in results.
        /// </summary>
        /// <param name="fieldID">Field ID</param>
        public void DeleteField( uint fieldID ) {
            if ( _fields.Contains( fieldID ) ) {
                _fields.Remove( fieldID );
            }
            else {
                throw new ArgumentException( string.Format( "Field {0} does not exist.", fieldID.ToString() ) );
            }
        }

        /// <summary>
        /// Delete multiple fields from the list to be returned in results.
        /// </summary>
        /// <param name="fieldIDs">Array of Field IDs</param>
        public void DeleteFields( uint[] fieldIDs ) {
            foreach ( var fieldID in fieldIDs ) {
                DeleteField( fieldID );
            }
        }

        /// <summary>
        /// Clear the list of fields to be returned in results.
        /// </summary>
        public void ClearFields() {
            _fields.Clear();
        }
    }
}
