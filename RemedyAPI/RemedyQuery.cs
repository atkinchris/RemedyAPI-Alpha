using BMC.ARSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RemedyAPI {
    public class RemedyQuery {

        #region Definitions
        // Connection Properties
        private string _server;
        private string _form;
        private string _username;
        private string _password;
        private uint _maxRecords = 500;

        // Filters
        private List<string> _groups = new List<string>();
        private List<int> _fields = new List<int>();
        private Dictionary<string, Query> _querys = new Dictionary<string, Query>();

        // BMC AR Server Objects
        Server server = new Server();
        #endregion

        #region Constructor
        /// <summary>
        /// RemedyQuery Constructor - called on creation of a new RemedyQuery object, and sets connection properties.
        /// Server and Form are set from default config, but can be changed with respective set methods.
        /// </summary>
        /// <param name="username">Username for connection</param>
        /// <param name="password">Password for connection</param>
        public RemedyQuery( string username, string password, string server = "a-rrm-ars-p", string form = "HPD:Help Desk" ) {
            SetUsername( username );
            SetPassword( password );
            SetServer( server );
            SetForm( form );
        }
        #endregion

        #region Set Methods
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
            else if ( !Regex.IsMatch( server, @"^[a-zA-Z0-9-]+$" ) ) {
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
            else if ( !Regex.IsMatch( form, @"^[a-zA-Z0-9: ]+$" ) ) {
                throw new ArgumentException( "Form name contains invalid characers." );
            }
            this._form = form;
        }
        #endregion

        #region Group Methods
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
        #endregion

        #region Field Methods
        /// <summary>
        /// Add a single field to be included in the results.
        /// </summary>
        /// <param name="fieldID">Field ID</param>
        public void AddField( int fieldID ) {
            _fields.Add( fieldID );
        }

        /// <summary>
        /// Add multiple fields to be included in the results.
        /// </summary>
        /// <param name="fieldIDs">Array of Field IDs</param>
        public void AddFields( int[] fieldIDs ) {
            foreach ( var fieldID in fieldIDs ) {
                AddField( fieldID );
            }
        }

        /// <summary>
        /// Delete a single field from the list to be returned in results.
        /// </summary>
        /// <param name="fieldID">Field ID</param>
        public void DeleteField( int fieldID ) {
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
        public void DeleteFields( int[] fieldIDs ) {
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

        /// <summary>
        /// Get an EntryListFieldList object contained all the defined fields.
        /// </summary>
        /// <returns>EntryListFieldList of fields</returns>
        private EntryListFieldList GetFieldList() {
            var fieldList = new EntryListFieldList();
            foreach ( var i in _fields ) {
                fieldList.AddField( i );
            }
            return fieldList;
        }
        #endregion

        #region Query Methods
        /// <summary>
        /// Add a single query to the list of available queries.
        /// All special characters must be escaped, and correct speech marks used, as per Remedy advanced search documentation.
        /// </summary>
        /// <param name="title">Query title</param>
        /// <param name="query">Query string</param>
        public void AddQuery( string title, string query ) {
            var queryObject = new Query( query );
            _querys.Add( title, queryObject );
        }

        /// <summary>
        /// Delete a single query from the list of available queries.
        /// </summary>
        /// <param name="title">Query title</param>
        public void DeleteQuery( string title ) {
            if ( _querys.ContainsKey( title ) ) {
                _querys.Remove( title );
            }
            else {
                throw new ArgumentException( string.Format( "Query {0} does not exist.", title ) );
            }
        }

        /// <summary>
        /// Get the formatted query for group filtering, based on group list. If no groups defined, returns empty.
        /// </summary>
        /// <returns>Group filter query in string format.</returns>
        private string GetGroupQuery() {
            if ( _groups.Count != 0 ) {
                var groupQuery = new StringBuilder();
                groupQuery.Append( "(" );
                foreach ( var group in _groups ) {
                    // 1000000217 = Assigned Group FieldID, converted to uint for performance.
                    groupQuery.AppendFormat( "(\'{0}\' = \"{1}\")", "1000000217", group );
                    if ( !group.Equals( _groups.Last() ) ) {
                        groupQuery.Append( " OR " );
                    }
                }
                groupQuery.Append( ")" );
                return groupQuery.ToString();
            }
            else {
                return string.Empty;
            }
        }
        #endregion

        #region Execution Methods
        /// <summary>
        /// Execute all querys against the Remedy server.
        /// </summary>
        /// <param name="maxRecords">Maximum number of records to return</param>
        public void ExecuteQuerys( ) {
            server.Login( _server, _username, _password );

            foreach ( var query in _querys.Values ) {
                RunQuery(query);
            }

            server.Logout();
        }

        /// <summary>
        /// Execute a specific query against the Remedy server.
        /// </summary>
        /// <param name="queryTitle">Query title</param>
        /// <param name="maxRecords">Maximum number of records to return</param>
        public void ExecuteQuery( string queryTitle ) {
            server.Login( _server, _username, _password );

            RunQuery(_querys[queryTitle]);

            server.Logout();
        }
        #endregion

        public void RunQuery( Query query ) {
            string queryString;
            if ( _groups.Count > 0 ) {
                queryString = string.Format( "{0} AND ({1})", GetGroupQuery(), query.queryString );
            }
            else {
                queryString = query.queryString;
            }
            query.results = server.GetListEntryWithFields( _form, queryString, GetFieldList(), 0, _maxRecords );
        }
    }
}
