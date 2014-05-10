using System.Collections.Generic;
using BMC.ARSystem;

namespace RemedyAPI {
    internal static class StringExtensions {
        public static bool IsNullOrBlank( this string text ) {
            return text == null || text.Trim().Length == 0;
        }
    }
}
