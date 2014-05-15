using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RemedyAPI {
    public abstract class DictionaryFilter : Dictionary<string, bool> {

        /// <summary>
        /// The scope of the filter.
        /// </summary>
        protected abstract string Scope { get; }
        /// <summary>
        /// The Regex validation to be applied when setting filter values.
        /// </summary>
        protected abstract string Validation { get; }

        /// <summary>
        /// Create new empty filter list.
        /// </summary>
        protected DictionaryFilter() { }
        /// <summary>
        /// Create new filter list with a single filter.
        /// </summary>
        /// <param name="filter">Filter to add</param>
        protected DictionaryFilter( string filter ) {
            Add( filter );
        }
        /// <summary>
        /// Create new filter list with multiple filters.
        /// </summary>
        /// <param name="filters">Filters to add</param>
        protected DictionaryFilter( string[] filters ) {
            Add( filters );
        }

        /// <summary>
        /// Add filter to filter list.
        /// </summary>
        /// <param name="filter">Filter value</param>
        /// <param name="exclude">If filter should exclude value</param>
        public new void Add( string filter, bool exclude = false ) {
            if ( String.IsNullOrWhiteSpace(filter) ) {
                throw new ArgumentException( "Filter must not be blank." );
            }
            if ( !Regex.IsMatch( filter, Validation ) ) {
                throw new ArgumentException( string.Format( "{0} contains invalid characers.", filter ) );
            }
            base.Add( filter, exclude );
        }
        /// <summary>
        /// Add multiple filters to filter list.
        /// </summary>
        /// <param name="filters">Filter values to add to list</param>
        /// <param name="exclude">If filter should exclude value</param>
        public void Add( string[] filters, bool exclude = false ) {
            foreach ( var group in filters ) {
                Add( group, exclude );
            }
        }
        /// <summary>
        /// Return filter list as query string.
        /// </summary>
        /// <returns>Filter list as query string.</returns>
        public override string ToString() {
            var parts = new[] { 
                String.Join( " OR ", this.Where( f => f.Value == false ).Select( f => String.Format( "\'{0}\' = \"{1}\"", Scope, f.Key ) ) ),
                String.Join( " AND ", this.Where( f => f.Value ).Select( f => String.Format( "\'{0}\' != \"{1}\"", Scope, f.Key ) ) )
            };
            return String.Join( " AND ", parts.Where( p => String.IsNullOrWhiteSpace(p) == false ).Select( p => String.Format( "({0})", p ) ) );
        }
    }

    public class Users : DictionaryFilter {

        protected override string Scope {
            get {
                return "Assignee";
            }
        }
        protected override string Validation {
            get {
                return @"^[a-zA-Z0-9\-\ ]+$";
            }
        }
    }

    public class Groups : DictionaryFilter {

        protected override string Scope {
            get {
                return "Assigned Group";
            }
        }
        protected override string Validation {
            get {
                return @"^[a-zA-Z0-9\:\-\&\ ]+$";
            }
        }
    }
}
