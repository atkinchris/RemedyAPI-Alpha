using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// This is a very lazy offline imitation of a Remedy Server.
/// It should be commented/excluded when using in real environment.

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
            this.EntryId = "INC000000" + ID;
        }

        public FieldValueList FieldValues {
            get {
                var results = new FieldValueList( 1 );
                results.Add( "Id", this.EntryId );
                results.Add( "Assigned Group", "CSDO" );
                results.Add( "Assignee", "Chris Atkin" );
                results.Add( "Summary", "Something broke" );
                results.Add( "Service Type", "User Service Restoration" );
                results.Add( "Status", "Resolved" );
                results.Add( "Submit Date", DateTime.Now.AddHours( -10 ) );
                results.Add( "Assigned", DateTime.Now.AddHours( -4 ) );
                results.Add( "Last Resolved Date", DateTime.Now.AddHours( -1 ) );
                return results;
            }
        }
    }
}
