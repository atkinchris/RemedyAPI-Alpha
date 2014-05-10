using BMC.ARSystem;
using System.Collections.Generic;

namespace RemedyAPI {
    public class Results : Dictionary<string, Result> {

        public Results( EntryFieldValueList efvl ) {
            foreach ( var result in efvl ) {
                this.Add(result.EntryId, new Result(result.FieldValues));
            }
        }
    }

    public class Result : Dictionary<string, object> {

        public Result( FieldValueList fieldValues ) {
            foreach ( var field in fieldValues.Keys ) {
                this.Add( field.ToString(), fieldValues[field] );
            }
        }
    }
}
