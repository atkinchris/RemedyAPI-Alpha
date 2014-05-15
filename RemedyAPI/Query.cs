using System;
using System.Collections.Generic;
using System.Linq;

namespace RemedyAPI {
    public class Query {
        public Groups Groups = new Groups();
        public Users Users = new Users();
        public IncidentTypes Types = IncidentTypes.User;
        public StatusTypes Status;
        public string Qualification;
        public Results Results;

        /// <summary>
        /// Create a new empty query.
        /// </summary>
        public Query() { }
        /// <summary>
        /// Create a new query with specified qualification.
        /// </summary>
        /// <param name="qualification">Query qualification as escaped string</param>
        public Query( string qualification ) {
            Qualification = qualification;
        }
        /// <summary>
        /// Create a new query with specified qualification and group include filter.
        /// </summary>
        /// <param name="qualification">Query qualification as escaped string</param>
        /// <param name="group">Group to include</param>
        public Query( string qualification, string group ) {
            Qualification = qualification;
            Groups.Add( group );
        }
        /// <summary>
        /// Create a new query with specified qualification and groups include filter.
        /// </summary>
        /// <param name="qualification">Query qualification as escaped string</param>
        /// <param name="groups">Groups to include</param>
        public Query( string qualification, string[] groups ) {
            Qualification = qualification;
            Groups.Add( groups );
        }

        /// <summary>
        /// Return query as formatted query string, including all its filters and qualifications.
        /// </summary>
        /// <returns>Formatted query string</returns>
        public override string ToString() {
            var parts = new List<string>
            {
                Groups.ToString(),
                Users.ToString(),
                Types.ToQuery(),
                Status.ToQuery(),
                Qualification
            };
            return String.Join( " AND ", parts.Where( p => String.IsNullOrWhiteSpace(p) == false ).Select( p => String.Format( "({0})", p ) ) );
        }

        static public Results GetGroupStack( Server server, string group ) {
            var query = new Query( null, group );
            server.ExecuteQuery( query );
            return query.Results;
        }
        static public Results GetGroupStack( Server server, string[] groups ) {
            var query = new Query( null, groups );
            server.ExecuteQuery( query );
            return query.Results;
        }
        static public int GetGroupDepth( Server server, string group ) {
            return GetGroupStack( server, group ).Count;
        }
        static public int GetGroupDepth( Server server, string[] groups ) {
            return GetGroupStack( server, groups ).Count;
        }
        static public Results GetUserStack( Server server, string user ) {
            var query = new Query();
            query.Users.Add( user );
            server.ExecuteQuery( query );
            return query.Results;
        }
        static public Results GetUserStack( Server server, string[] users ) {
            var query = new Query();
            query.Users.Add( users );
            server.ExecuteQuery( query );
            return query.Results;
        }
        static public int GetUserDepth( Server server, string user ) {
            return GetUserStack( server, user ).Count;
        }
        static public int GetUserDepth( Server server, string[] users ) {
            return GetUserStack( server, users ).Count;
        }
        static public Results GetGroupResolvedTodayStack( Server server, string group ) {
            var qualification = string.Format( "(\'{0}\' > \"{1}\")", "Last Resolved Date", DateTime.Today.ToShortDateString() );
            var query = new Query( qualification, group ) { Status = StatusTypes.Closed };
            server.ExecuteQuery( query );
            return query.Results;
        }
        static public Results GetGroupResolvedTodayStack( Server server, string[] groups ) {
            var qualification = string.Format( "(\'{0}\' > \"{1}\")", "Last Resolved Date", DateTime.Today.ToShortDateString() );
            var query = new Query( qualification, groups ) { Status = StatusTypes.Closed };
            server.ExecuteQuery( query );
            return query.Results;
        }
        static public int GetGroupResolvedTodayDepth( Server server, string group ) {
            return GetGroupResolvedTodayStack( server, group ).Count;
        }
        static public int GetGroupResolvedTodayDepth( Server server, string[] groups ) {
            return GetGroupResolvedTodayStack( server, groups ).Count;
        }
        static public Results GetUserResolvedTodayStack( Server server, string user ) {
            var qualification = string.Format( "(\'{0}\' > \"{1}\")", "Last Resolved Date", DateTime.Today.ToShortDateString() );
            var query = new Query( qualification ) { Status = StatusTypes.Closed };
            query.Users.Add( user );
            server.ExecuteQuery( query );
            return query.Results;
        }
        static public Results GetUserResolvedTodayStack( Server server, string[] users ) {
            var qualification = string.Format( "(\'{0}\' > \"{1}\")", "Last Resolved Date", DateTime.Today.ToShortDateString() );
            var query = new Query( qualification ) { Status = StatusTypes.Closed };
            query.Users.Add( users );
            server.ExecuteQuery( query );
            return query.Results;
        }
        static public int GetUserResolvedTodayDepth( Server server, string group ) {
            return GetUserResolvedTodayStack( server, group ).Count;
        }
        static public int GetUserResolvedTodayDepth( Server server, string[] groups ) {
            return GetUserResolvedTodayStack( server, groups ).Count;
        }
        static public Results GetSubmittedTodayStack( Server server, string group ) {
            var qualification = string.Format( "(\'{0}\' > \"{1}\")", "Submit Date", DateTime.Today.ToShortDateString() );
            var query = new Query( qualification, group ) { Status = StatusTypes.All };
            server.ExecuteQuery( query );
            return query.Results;
        }
        static public Results GetSubmittedTodayStack( Server server, string[] groups ) {
            var qualification = string.Format( "(\'{0}\' > \"{1}\")", "Submit Date", DateTime.Today.ToShortDateString() );
            var query = new Query( qualification, groups ) { Status = StatusTypes.All };
            server.ExecuteQuery( query );
            return query.Results;
        }
        static public int GetSubmittedTodayDepth( Server server, string group ) {
            return GetSubmittedTodayStack( server, group ).Count;
        }
        static public int GetSubmittedTodayDepth( Server server, string[] groups ) {
            return GetSubmittedTodayStack( server, groups ).Count;
        }
        static public Dictionary<DateTime, int> GetSubmittedTodayGrouped( Server server, string[] groups, int interval = 60 ) {
            var results = GetSubmittedTodayStack( server, groups );

            var output = new Dictionary<DateTime, int>();
            for ( var i = DateTime.Today; i < DateTime.Now; i = i.AddMinutes( interval ) )
            {
                DateTime i1 = i;
                output.Add( i, results.Count( r => r.Value.Submitted < i1 ) );
            }

            return output;
        }
        static public Dictionary<DateTime, int> GetResolvedTodayGrouped( Server server, string[] groups, int interval = 60 ) {
            var results = GetSubmittedTodayStack( server, groups );

            var output = new Dictionary<DateTime, int>();
            for ( var i = DateTime.Today; i < DateTime.Now; i = i.AddMinutes( interval ) )
            {
                DateTime i1 = i;
                output.Add( i, results.Count( r => r.Value.Resolved < i1 ) );
            }

            return output;
        }
        static public Dictionary<DateTime, int> GetUserResolvedTodayGrouped( Server server, string[] users, int interval = 60 ) {
            var results = GetUserResolvedTodayStack( server, users );

            var output = new Dictionary<DateTime, int>();
            for ( var i = DateTime.Today; i < DateTime.Now; i = i.AddMinutes( interval ) )
            {
                DateTime i1 = i;
                output.Add( i, results.Count( r => r.Value.Submitted < i1 ) );
            }

            return output;
        }
    }
}
