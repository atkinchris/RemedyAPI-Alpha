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
        public DateTime Created;
        public DateTime Assigned;
        public DateTime Resolved;

        public Result( FieldValueList fieldValues ) {
            Id = fieldValues["Id"] as string;
            AssignedGroup = fieldValues["Assigned Group"] as string;
            Assignee = fieldValues["Assignee"] as string;
            Summary = fieldValues["Summary"] as string;
            Type = fieldValues["Service Type"] as string;
            Status = fieldValues["Status"] as string;
            Created = Convert.ToDateTime( fieldValues["Created"] );
            Assigned = Convert.ToDateTime( fieldValues["Assigned"] );
            Resolved = Convert.ToDateTime( fieldValues["Resolved"] );
        }
    }
}
