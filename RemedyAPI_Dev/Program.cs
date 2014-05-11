using RemedyAPI;
using System;

namespace RemedyAPI_Dev {
    class Program {
        static void Main(string[] args) {

            var server = new Server( "Chris", "Password" );
            server.Login();

            var query = new Query();
            server.ExecuteQuery( query );

            foreach ( var result in query.results.Values ) {
                Console.WriteLine( result.Id + ": " + result.Created );
            }
            Console.ReadLine();
        }
    }
}
