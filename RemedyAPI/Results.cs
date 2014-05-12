using BMC.ARSystem;
using System;
using System.Collections.Generic;

namespace RemedyAPI {
    public class Results : Dictionary<string, Result> {

        public Results( EntryFieldValueList efvl ) {
            foreach ( var result in efvl ) {
                Add( result.EntryId, new Result( result.FieldValues ) );
            }
        }
    }

    public class Result {

        public string Id;
        public string AssignedGroup;
        public string Assignee;
        public string Summary;
        public string Type;
        public string Status;
        public DateTime Submitted;
        public DateTime Assigned;
        public DateTime Resolved;

        public Result( FieldValueList fieldValues ) {
            Id = fieldValues[ 1000000161 ] as string;
            AssignedGroup = fieldValues[ 1000000217 ] as string;
            Assignee = fieldValues[ 1000000218 ] as string;
            Summary = fieldValues[ 1000000000 ] as string;
            Type = fieldValues[ 1000003009 ] as string;
            Status = fieldValues[ 1000000099 ] as string;
            Submitted = Convert.ToDateTime( fieldValues[ 3 ] as string );
            Resolved = Convert.ToDateTime( fieldValues[ 1000000563 ] as string );
        }
    }
}
