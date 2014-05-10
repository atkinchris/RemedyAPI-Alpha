using System;
using System.Collections.Generic;
using System.Linq;

namespace RemedyAPI {
    public class Query {
        public Groups groups = new Groups();
        public Users users = new Users();
        public IncidentTypes types;
        public string qualification;
        public Results results;
        public Fields fields;

        public override string ToString() {
            var parts = new List<string>() {
                groups.ToString(),
                users.ToString(),
                types.ToQuery(),
                qualification
            };            
            return String.Join( " AND ", parts.Where( p => p.IsNullOrBlank() == false ).Select( p => String.Format( "({0})", p ) ) );
        }
    }
}
