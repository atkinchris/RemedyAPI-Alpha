using System;
using System.Collections.Generic;

namespace BMC.ARSystem {

    static internal class RNG {
        static public Random n = new Random();
    }

    class Server {
        public EntryFieldValueList GetListEntryWithFields( string form, string qualification, int[] fields, uint startRecord, uint maxRecord ) {
            var results = new EntryFieldValueList();
            for ( int i = 156; i <= 180; i++ ) {
                results.Add( new FieldValueList( i ) );
            }
            return results;
        }

        public void Login( string server, string username, string password ) { }
        public void Logout() { }
    }

    public class EntryFieldValueList : List<FieldValueList> {
    }

    public class FieldValueList : Dictionary<int, object> {

        public string EntryId;

        public FieldValueList( int ID ) {
            EntryId = "INC000000" + ID;
        }

        public FieldValueList FieldValues {
            get {
                var results = new FieldValueList( 1 )
                {
                    {1000000161, EntryId},
                    {1000000217, "CSDO"},
                    {1000000218, "Chris Atkin"},
                    {1000000000, "Something broke"},
                    {1000003009, "User Service Restoration"},
                    {1000000164, "Resolved"},
                    {"Submit Date", DateTime.Now.AddMinutes( -RNG.n.Next(0, 4*60))},
                    {"Assigned", DateTime.Now.AddHours(-4)},
                    {"Last Resolved Date", DateTime.Now.AddMinutes( -RNG.n.Next(0, 4*60))}
                };
                return results;
            }
        }
    }
}
