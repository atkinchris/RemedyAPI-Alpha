using BMC.ARSystem;
using System;

namespace RemedyAPI {
    public class Incident {
        public string EntryId;
        public string IncidentNumber;
        public string AssignedGroup;
        public string Assignee;
        public string Summary;
        public IncidentType Type;
        public Status Status;
        public DateTime Submitted;
        public DateTime? Resolved;

        public Incident( FieldValueList fV ) {
            EntryId = fV[ FieldId.EntryId ].ToString();
            IncidentNumber = fV[ FieldId.IncidentNumber ].ToString();
            AssignedGroup = fV[ FieldId.AssignedGroup ].ToString();
            Assignee = fV[ FieldId.Assignee ].ToString();
            Summary = fV[ FieldId.Summary ].ToString();
            Type = (IncidentType)fV[ FieldId.Type ];
            Status = (Status)fV[ FieldId.Status ];
            Submitted = Convert.ToDateTime( fV[ FieldId.Submitted ] );
            if ( fV[ FieldId.Resolved ] != DBNull.Value ) {
                Resolved = Convert.ToDateTime( fV[ FieldId.Resolved ] );
            } else {
                Resolved = null;
            }
        }
    }

    public enum IncidentType {
        UserRestoration,
        UserRequest,
        InfrastructureRestoration,
        InfrastructureRequest
    }

    public enum Status {
        New,
        Assigned,
        InProgress,
        Pending,
        Resolved,
        Closed,
        Cancelled
    }

    public enum FieldId {
        EntryId = 1,
        IncidentNumber = 1000000161,
        AssignedGroup = 1000000217,
        Assignee = 1000000218,
        Summary = 1000000000,
        Type = 1000000099,
        Status = 7,
        Submitted = 3,
        Resolved = 1000000563
    }
}
