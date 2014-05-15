using System;

namespace RemedyAPI {

    /// <summary>
    /// Enum of incident types to filter by.
    /// </summary>
    public enum IncidentTypes {
        All,
        User,
        Infrastructure
    }

    static class IncidentTypesExtensions {

        /// <summary>
        /// Extension to IncidentTypes to return type as query string.
        /// </summary>
        /// <param name="type">this IncidentTypes</param>
        /// <returns>IncidentType as query string</returns>
        internal static string ToQuery( this IncidentTypes type ) {
            switch ( type ) {
                case IncidentTypes.Infrastructure:
                    return String.Format( "(\'{0}\' >= \"{1}\")", "Service Type", "Infrastructure Restoration" );
                case IncidentTypes.User:
                    return String.Format( "(\'{0}\' < \"{1}\")", "Service Type", "Infrastructure Restoration" );
                default:
                    return String.Empty;
            }
        }
    }
}
