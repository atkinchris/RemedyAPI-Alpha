using RemedyAPI;
using System;

namespace RemedyAPI_Dev {
    class Program {
        static void Main(string[] args) {
            var query = new Query();
            query.users.Add( "Chris" );
            query.users.Add( "Dan" );
            query.users.Add( "Jeff", true );
            query.users.Add( "Cake", true );
            Console.WriteLine(query.ToString());
            Console.ReadLine();
        }
    }
}
