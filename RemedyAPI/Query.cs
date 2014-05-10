using System;
using System.Collections.Generic;
using System.Linq;

namespace RemedyAPI {
    public class Query {
        public Groups groups = new Groups();
        public Users users = new Users();
        public Fields fields = new Fields();
        public IncidentTypes types = IncidentTypes.Incidents;
        public StatusTypes status = StatusTypes.Open;
        public string qualification;
        public Results results;

        public Query() { }
        public Query( string qualification ) {
            this.qualification = qualification;
        }
        public Query( string qualification, string group ) {
            this.qualification = qualification;
            this.groups.Add( group );
        }
        public Query( string qualification, string[] groups ) {
            this.qualification = qualification;
            this.groups.Add( groups );
        }

        public override string ToString() {
            var parts = new List<string>() {
                groups.ToString(),
                users.ToString(),
                types.ToQuery(),
                status.ToQuery(),
                qualification
            };            
            return String.Join( " AND ", parts.Where( p => p.IsNullOrBlank() == false ).Select( p => String.Format( "({0})", p ) ) );
        }
    }
}
