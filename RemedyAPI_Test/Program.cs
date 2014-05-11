using RemedyAPI;
using System;

namespace RemedyAPI_Test {
    class Program {
        static void Main() {

            var server = new Server( "chris", "atkin" ) {
                cacheTime = 5
            };

            Query.GetUserResolvedTodayGrouped( server, new [] {"chris"}, 15 );
            var query = new Query( null, "chris" );
            for ( int i = 1; i > 0; i++ ) {
                server.ExecuteQuery( query );
                Console.WriteLine( DateTime.Now + ":: " + 
                    query.results["INC000000160"].Submitted );
                Console.ReadLine();
            }
        }
    }
}
