using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemedyAPI {

    public enum IncidentTypes {
        All,
        Incidents,
        Alerts
    }

    public static class IncidentTypesExtensions {
        public static string ToQuery( this IncidentTypes type ) {
            switch ( type ) {
                case IncidentTypes.Alerts:
                    return String.Format( "\'{0}\' >= \"{1}\"", "Service Type", "Infrastructure Restoration" );
                case IncidentTypes.Incidents:
                    return String.Format( "\'{0}\' < \"{1}\"", "Service Type", "Infrastructure Restoration" );;
                case IncidentTypes.All:
                    return String.Empty;
                default:
                    return String.Empty;
            }
        }
    }
}
