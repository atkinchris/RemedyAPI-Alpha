using System;

namespace RemedyAPI {

    /// <summary>
    /// Enum of status types to filter by.
    /// </summary>
    public enum StatusTypes {
        All,
        Open,
        Closed
    }

    static class StatusTypesExtensions {

        /// <summary>
        /// Extension to StatusTypes to return type as query string.
        /// </summary>
        /// <param name="status">this StatusTypes</param>
        /// <returns>StatusTypes as query string</returns>
        internal static string ToQuery( this StatusTypes status ) {
            switch ( status ) {
                case StatusTypes.Open:
                    return String.Format( "(\'{0}\' < \"{1}\")", "Status", "Resolved" );
                case StatusTypes.Closed:
                    return String.Format( "(\'{0}\' >= \"{1}\")", "Status", "Resolved" );
                case StatusTypes.All:
                    return String.Empty;
                default:
                    return String.Empty;
            }
        }
    }
}
