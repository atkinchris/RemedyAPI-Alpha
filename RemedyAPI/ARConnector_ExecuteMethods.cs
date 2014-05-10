using System.Collections.Generic;

namespace RemedyAPI {
    public partial class ARConnector {

        /// <summary>
        /// Execute all queries against the Remedy server.
        /// </summary>
        public void ExecuteQueries() {
            server.ExecuteQuery( queries, groups, fields );
        }

        /// <summary>
        /// Execute a specific query against the Remedy server.
        /// </summary>
        /// <param name="title">Query title</param>
        public void ExecuteQuery( string title ) {
            var query = queries.GetQuery( title );
            server.ExecuteQuery( query, groups, fields );
        }

        /// <summary>
        /// Retrieve the number of incidents for a group's queue that are at a status earlier than resolved.
        /// </summary>
        /// <param name="queue">Group name</param>
        /// <returns>Int count of incidents</returns>
        public int GetQueueDepth( string queue ) {
            var query = new Query( string.Format( "(\'{0}\' < \"{1}\")", "Status", "Resolved" ) );
            server.ExecuteQuery( query, new Groups( queue ), new Fields() );
            return query.results.Count;
        }        

        public List<Result> GetIncidents( string group ) {
            var groups = new Groups( group );
            var queryString = string.Format( "{0} AND ({1})", groups.ToString(), query.ToString() );
        }
    }
}
