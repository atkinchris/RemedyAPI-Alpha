using RemedyAPI;
using System;

namespace RemedyAPI_Dev {
    class Program {
        static void Main(string[] args) {

            var server = new Server( "Chris", "Password" );
            server.Login();
            Console.WriteLine( server.cacheTime );
            Console.ReadLine();
        }
    }
}
