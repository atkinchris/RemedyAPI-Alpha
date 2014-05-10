﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemedyAPI {
    public enum StatusTypes {
        All,
        Open,
        Closed
    }

    static class StatusTypesExtensions {
        internal static string ToQuery( this StatusTypes status ) {
            switch ( status ) {
                case StatusTypes.Open:
                    return String.Format( "(\'{0}\' >= \"{1}\")", "Service Type", "Infrastructure Restoration" );
                case StatusTypes.Closed:
                    return String.Format( "(\'{0}\' < \"{1}\")", "Service Type", "Infrastructure Restoration" ); ;
                case StatusTypes.All:
                    return String.Empty;
                default:
                    return String.Empty;
            }
        }
    }
}
