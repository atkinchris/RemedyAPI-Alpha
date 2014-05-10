﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RemedyAPI {
    public abstract class DictionaryFilter : Dictionary<string, bool> {

        protected abstract string scope { get; }
        protected abstract string validation { get; }

        public DictionaryFilter() { }
        public DictionaryFilter( string filter ) {
            this.Add( filter );
        }
        public DictionaryFilter( string[] filters ) {
            this.Add( filters );
        }

        public new void Add( string filter, bool exclude = false ) {
            if ( filter.IsNullOrBlank() ) {
                throw new ArgumentException( "Filter must not be blank." );
            }
            else if ( !Regex.IsMatch( filter, validation ) ) {
                throw new ArgumentException( string.Format( "Group name contains invalid characers.", filter ) );
            }
            base.Add( filter, exclude );
        }
        public void Add( string[] filters, bool exclude = false ) {
            foreach ( var group in filters ) {
                Add( group, exclude );
            }
        }
        public override string ToString() {
            var parts = new string[] { 
                String.Join( " OR ", this.Where( f => f.Value == false ).Select( f => String.Format( "\'{0}\' = \"{1}\"", scope, f.Key ) ) ),
                String.Join( " AND ", this.Where( f => f.Value == true ).Select( f => String.Format( "\'{0}\' != \"{1}\"", scope, f.Key ) ) )
            };
            return String.Join( " AND ", parts.Where( p => p.IsNullOrBlank() == false ).Select( p => String.Format( "({0})", p ) ) );
        }
    }

    public class Users : DictionaryFilter {

        protected override string scope {
            get {
                return "Assignee";
            }
        }
        protected override string validation {
            get {
                return @"^[a-zA-Z0-9\-\ ]+$";
            }
        }
    }

    public class Groups : DictionaryFilter {

        protected override string scope {
            get {
                return "Assigned Group";
            }
        }
        protected override string validation {
            get {
                return  @"^[a-zA-Z0-9\:\-\&\ ]+$";
            }
        }
    }
}