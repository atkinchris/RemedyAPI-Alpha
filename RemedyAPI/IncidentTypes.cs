using System;

namespace RemedyAPI {

    public enum IncidentTypes {
        All,
        Incidents,
        Alerts
    }

    static class IncidentTypesExtensions {
        internal static string ToQuery( this IncidentTypes type ) {
            switch ( type ) {
                case IncidentTypes.Alerts:
                    return String.Format( "(\'{0}\' >= \"{1}\")", "Service Type", "Infrastructure Restoration" );
                case IncidentTypes.Incidents:
                    return String.Format( "(\'{0}\' < \"{1}\")", "Service Type", "Infrastructure Restoration" );;
                case IncidentTypes.All:
                    return String.Empty;
                default:
                    return String.Empty;
            }
        }
    }
}
