using System;
using System.Runtime.Caching;

namespace RemedyAPI_Test {
    class Program {

        static void Main() {

            var server = new BMC.ARSystem.Server();

            server.Login( "a-rrm-ars-p", Credentials.GetUsername(), Credentials.GetPassword() );

            var info = string.Format( "(\'{0}\' < \"{1}\") AND (\'{2}\' = \"{3}\")", "Status", "Resolved", "Assignee", "Christopher Atkin");

            var results = server.GetListEntry( "HPD:Help Desk", info );

        }
    }
}
