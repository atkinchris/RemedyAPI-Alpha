using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RemedyAPI;

namespace RemedyAPI_Dev {
    class Program {
        static void Main(string[] args) {

            var remedy = new ARConnector( "Christopher.Atkin", "Daniel33");
            var groups = new string[] {
                "Collaboration Services",
                "CS Deploy & Operate",
                "CS Deploy and Operate",
                "End User Devices"
            };
            remedy.AddGroups(groups);

            var queryString = new StringBuilder();
            queryString.AppendFormat("(\'{0}\' < \"{1}\")", "Status", "Resolved");
            queryString.AppendFormat(" AND (\'{0}\' < \"{1}\")", "Service Type", "Infrastructure Restoration");
            queryString.AppendFormat(" AND (\'{0}\' <> \"{1}\")", "Assignee", "Ian Taylor");
            remedy.AddQuery("Outstanding", queryString.ToString());
            remedy.AddField(1000000080);
            remedy.ExecuteQueries();
        }
    }
}
