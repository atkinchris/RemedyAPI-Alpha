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

        /// <summary>
        /// Create a new empty query.
        /// </summary>
        public Query() { }
        /// <summary>
        /// Create a new query with specified qualification.
        /// </summary>
        /// <param name="qualification">Query qualification as escaped string</param>
        public Query( string qualification ) {
            this.qualification = qualification;
        }
        /// <summary>
        /// Create a new query with specified qualification and group include filter.
        /// </summary>
        /// <param name="qualification">Query qualification as escaped string</param>
        /// <param name="group">Group to include</param>
        public Query( string qualification, string group ) {
            this.qualification = qualification;
            groups.Add( group );
        }
        /// <summary>
        /// Create a new query with specified qualification and groups include filter.
        /// </summary>
        /// <param name="qualification">Query qualification as escaped string</param>
        /// <param name="groups">Groups to include</param>
        public Query( string qualification, string[] groups ) {
            this.qualification = qualification;
            this.groups.Add( groups );
        }

        /// <summary>
        /// Return query as formatted query string, including all its filters and qualifications.
        /// </summary>
        /// <returns>Formatted query string</returns>
        public override string ToString() {
            var parts = new List<string>
            {
                groups.ToString(),
                users.ToString(),
                types.ToQuery(),
                status.ToQuery(),
                qualification
            };
            return String.Join( " AND ", parts.Where( p => String.IsNullOrWhiteSpace(p) == false ).Select( p => String.Format( "({0})", p ) ) );
        }

        static public Results GetGroupStack( Server server, string group ) {
            var query = new Query( null, group );
            server.ExecuteQuery( query );
            return query.results;
        }
        static public Results GetGroupStack( Server server, string[] groups ) {
            var query = new Query( null, groups );
            server.ExecuteQuery( query );
            return query.results;
        }
        static public int GetGroupDepth( Server server, string group ) {
            return GetGroupStack( server, group ).Count;
        }
        static public int GetGroupDepth( Server server, string[] groups ) {
            return GetGroupStack( server, groups ).Count;
        }
        static public Results GetUserStack( Server server, string user ) {
            var query = new Query();
            query.users.Add( user );
            server.ExecuteQuery( query );
            return query.results;
        }
        static public Results GetUserStack( Server server, string[] users ) {
            var query = new Query();
            query.users.Add( users );
            server.ExecuteQuery( query );
            return query.results;
        }
        static public int GetUserDepth( Server server, string user ) {
            return GetUserStack( server, user ).Count;
        }
        static public int GetUserDepth( Server server, string[] users ) {
            return GetUserStack( server, users ).Count;
        }
        static public Results GetResolvedTodayStack( Server server, string group ) {
            var qualification = string.Format( "(\'{0}\' > \"{1}\")", "Last Resolved Date", DateTime.Today.ToShortDateString() );
            var query = new Query( qualification, group ) { status = StatusTypes.Closed };
            server.ExecuteQuery( query );
            return query.results;
        }
        static public Results GetResolvedTodayStack( Server server, string[] groups ) {
            var qualification = string.Format( "(\'{0}\' > \"{1}\")", "Last Resolved Date", DateTime.Today.ToShortDateString() );
            var query = new Query( qualification, groups ) { status = StatusTypes.Closed };
            server.ExecuteQuery( query );
            return query.results;
        }
        static public int GetResolvedTodayDepth( Server server, string group ) {
            return GetResolvedTodayStack( server, group ).Count;
        }
        static public int GetResolvedTodayDepth( Server server, string[] groups ) {
            return GetResolvedTodayStack( server, groups ).Count;
        }
        static public Results GetSubmittedTodayStack( Server server, string group ) {
            var qualification = string.Format( "(\'{0}\' > \"{1}\")", "Submit Date", DateTime.Today.ToShortDateString() );
            var query = new Query( qualification, group ) { status = StatusTypes.All };
            server.ExecuteQuery( query );
            return query.results;
        }
        static public Results GetSubmittedTodayStack( Server server, string[] groups ) {
            var qualification = string.Format( "(\'{0}\' > \"{1}\")", "Submit Date", DateTime.Today.ToShortDateString() );
            var query = new Query( qualification, groups ) { status = StatusTypes.All };
            server.ExecuteQuery( query );
            return query.results;
        }
        static public int GetSubmittedTodayDepth( Server server, string group ) {
            return GetSubmittedTodayStack( server, group ).Count;
        }
        static public int GetSubmittedTodayDepth( Server server, string[] groups ) {
            return GetSubmittedTodayStack( server, groups ).Count;
        }
    }
}
