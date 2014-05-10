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
                results.Add( new FieldValueList(i) );
            }
            return results;
        }

        public void Login( string server, string username, string password ) { }

        public void Logout() { }

    }

    public class EntryFieldValueList : List<FieldValueList> {
    }

    public class FieldValueList : Dictionary<string, string> {

        public string EntryId;

        public FieldValueList( int ID ) {
            this.EntryId = "INC000000" + ID;
        }

        public FieldValueList FieldValues {
            get {                
                var results = new FieldValueList( 1 );
                results.Add( "INCID", "TEST" );
                return results;
            }
        }
    }
}
