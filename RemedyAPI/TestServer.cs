using System;
using System.Collections.Generic;

namespace BMC.ARSystem {
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

    public class FieldValueList : Dictionary<string, object> {

        public string EntryId;

        public FieldValueList( int ID ) {
            EntryId = "INC000000" + ID;
        }

        public FieldValueList FieldValues {
            get {
                var results = new FieldValueList( 1 )
                {
                    {"Id", EntryId},
                    {"Assigned Group", "CSDO"},
                    {"Assignee", "Chris Atkin"},
                    {"Summary", "Something broke"},
                    {"Service Type", "User Service Restoration"},
                    {"Status", "Resolved"},
                    {"Submit Date", DateTime.Now.AddHours(-10)},
                    {"Assigned", DateTime.Now.AddHours(-4)},
                    {"Last Resolved Date", DateTime.Now.AddHours(-1)}
                };
                return results;
            }
        }
    }
}
