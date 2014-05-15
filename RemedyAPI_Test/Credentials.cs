using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace RemedyAPI_Test {
    static class Credentials {

        static public string GetUsername() {
            string username;
            var windowsIdentity = WindowsIdentity.GetCurrent();
            if ( windowsIdentity != null ) {
                var identity = windowsIdentity.Name;
                username = identity.Substring( identity.IndexOf( @"\", StringComparison.Ordinal ) + 1 );
                Console.WriteLine( "Username: " + username );
            }
            else {
                Console.WriteLine( "Username:" );
                username = Console.ReadLine();
            }
            return username;
        }

        static public string GetPassword() {
            Console.WriteLine( "Password:" );
            var password = Console.ReadLine();
            Console.Clear();
            return password;
        }

    }
}
