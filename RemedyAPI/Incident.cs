using BMC.ARSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemedyAPI {
    public class Incident {
        public string entryId;
        public string incidentNumber;
        public string assignedGroup;
        public string assignee;
        public string summary;
        public IncidentType type;
        public Status status;
        public DateTime submitted;
        public DateTime? resolved;

        public Incident( FieldValueList fV ) {
            entryId = fV[ FieldId.EntryId ].ToString();
            incidentNumber = fV[ FieldId.IncidentNumber ].ToString();
            assignedGroup = fV[ FieldId.AssignedGroup ].ToString();
            assignee = fV[ FieldId.Assignee ].ToString();
            summary = fV[ FieldId.Summary ].ToString();
            type = (IncidentType)fV[ FieldId.Type ];
            status = (Status)fV[ FieldId.Status ];
            submitted = Convert.ToDateTime( fV[ FieldId.Submitted ] );
            if ( fV[ FieldId.Resolved ] != DBNull.Value ) {
                resolved = Convert.ToDateTime( fV[ FieldId.Resolved ] );
            } else {
                resolved = null;
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
