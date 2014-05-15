using BMC.ARSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RemedyAPI {
    public class Results : Dictionary<string, Incident> {
        public Results( EntryFieldValueList efvl ) {
            foreach ( var result in efvl ) {
                Add( result.EntryId, new Incident( result.FieldValues ) );
            }
        }
    }
}
