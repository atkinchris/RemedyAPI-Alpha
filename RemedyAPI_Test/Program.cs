using RemedyAPI;
using System;

namespace RemedyAPI_Test {
    class Program {
        static void Main() {

            var server = new Server( "chris", "atkin" ) {
                cacheTime = 5
            };
            var query = new Query( null, "chris" );
            for ( int i = 0; i < 100; i++ ) {
                server.ExecuteQuery( query );
                Console.WriteLine( query.results["INC000000160"].Submitted );
                Console.ReadLine();
            }
        }
    }
}
