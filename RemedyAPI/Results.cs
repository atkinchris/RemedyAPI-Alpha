using BMC.ARSystem;
using System;
using System.Collections.Generic;

namespace RemedyAPI {
    public class Results : Dictionary<string, Result> {

        public Results( EntryFieldValueList efvl ) {
            foreach ( var result in efvl ) {
                this.Add( result.EntryId, new Result( result.FieldValues ) );
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
            this.Id = fieldValues["Id"] as string;
            this.AssignedGroup = fieldValues["Assigned Group"] as string;
            this.Assignee = fieldValues["Assignee"] as string;
            this.Summary = fieldValues["Summary"] as string;
            this.Type = fieldValues["Service Type"] as string;
            this.Status = fieldValues["Status"] as string;
            this.Created = Convert.ToDateTime( fieldValues["Created"] );
            this.Assigned = Convert.ToDateTime( fieldValues["Assigned"] );
            this.Resolved = Convert.ToDateTime( fieldValues["Resolved"] );
        }
    }
}
