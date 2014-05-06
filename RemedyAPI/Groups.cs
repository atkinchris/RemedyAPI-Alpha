using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RemedyAPI {
    public class Groups {

        private List<string> _groups = new List<string>();

        /// <summary>
        /// Add a single group name to the list of groups to filter by.
        /// </summary>
        /// <param name="group">Group name</param>
        public void Add( string group ) {
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
                Add( group );
            }
        }

        /// <summary>
        /// Delete a single group from the list of groups to filter by.
        /// </summary>
        /// <param name="group">Group name</param>
        public void Delete( string group ) {
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
        public void Delete( string[] groups ) {
            foreach ( var group in groups ) {
                Delete( group );
            }
        }

        /// <summary>
        /// Clear the list of groups to filter by.
        /// </summary>
        public void Clear() {
            _groups.Clear();
        }

        /// <summary>
        /// Get the formatted query for group filtering, based on group list. If no groups defined, returns empty.
        /// </summary>
        /// <returns>Group filter query in string format.</returns>
        public override string ToString() {
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
    }
}
