using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RemedyAPI {
    public class Query {
        public Groups groups = new Groups();
        public Users users = new Users();
        public IncidentTypes types;
        public Results results;

        public enum IncidentTypes {
            All,
            Incidents,
            Alerts
        }

        public override string ToString() {
            var parts = new List<string>() {
                groups.ToString(),
                users.ToString()
            };            
            return String.Join( " AND ", parts.Where( p => p.IsNullOrBlank() == false ).Select( p => String.Format( "{0}", p ) ) );
        }

        public int ToInt() {
            return results.Count;
        }
    }
}
