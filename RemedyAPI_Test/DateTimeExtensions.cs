using System;

namespace RemedyAPI_Test {
    static class DateTimeExtensions {
        public static long ToJavaScriptTimestamp( this DateTime input ) {
            return (long)input.Subtract( new DateTime( 1970, 1, 1 ) ).TotalMilliseconds;
        }
    }
}
