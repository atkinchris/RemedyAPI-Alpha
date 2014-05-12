using RemedyAPI;
using System;
using System.Linq;
using System.Security.Principal;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace RemedyAPI_Test {
    class Program {
        static void Main() {

            string username;
            var windowsIdentity = WindowsIdentity.GetCurrent();
            if ( windowsIdentity != null ) {
                var identity = windowsIdentity.Name;
                username = identity.Substring( identity.IndexOf( @"\", StringComparison.Ordinal ) + 1 );
                Console.WriteLine( "Username: " + username );
            } else {
                Console.WriteLine( "Username:" );
                username = Console.ReadLine();
            }
            Console.WriteLine( "Password:" );
            var password = Console.ReadLine();
            Console.Clear();

            var groups = new[] {
                "Collaboration Services",
                "CS Deploy & Operate",
                "CS Deploy and Operate",
                "End User Devices"
            };

            var server = new Server( username, password );

            string date = DateTime.Today.AddDays( -21 ).ToShortDateString();
            var query = new Query( string.Format( "(\'{0}\' > \"{1}\")", "Last Resolved Date", date ) );
            query.users.Add( "Christopher Atkin" );
            query.status = StatusTypes.Closed;
            server.ExecuteQuery( query );

            var output = new Dictionary<string, int>();
            for ( TimeSpan i = TimeSpan.Zero; i < TimeSpan.FromDays( 1 ); i = i.Add( TimeSpan.FromMinutes( 15 ) ) ) {
                output.Add( i.Hours + ":" + i.Minutes, query.results.Count( r => r.Value.Resolved.TimeOfDay < i ) );
            }

            var stringOutput = new StringBuilder();
            stringOutput.AppendFormat( "{0},{1}{2}", "Time", "Count", Environment.NewLine );
            foreach ( var result in output ) {
                stringOutput.AppendFormat( "{0},{1}{2}", result.Key, result.Value, Environment.NewLine );
            }
            using ( StreamWriter sw = File.AppendText( @"results.csv" ) ) {
                sw.WriteLine( stringOutput.ToString() );
            }

            Console.WriteLine( query.results.Count );
            Console.ReadLine();
        }
    }
}
