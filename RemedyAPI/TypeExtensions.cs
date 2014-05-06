﻿using BMC.ARSystem;
using System.Collections.Generic;

namespace RemedyAPI {
    static class TypeExtensions {
        public static bool IsNullOrBlank( this string text ) {
            return text == null || text.Trim().Length == 0;
        }

        public static List<Result> ToResultsList( this EntryFieldValueList efvl ) {
            var parsedResults = new List<Result>();
            foreach ( var result in efvl ) {
                var parsedResult = new Result() {
                    entryID = result.EntryId
                };
                foreach ( var field in result.FieldValues.Keys ) {
                    parsedResult.fields.Add( field.ToString(), result.FieldValues[field] );
                }
            }
            return parsedResults;
        }
    }
}
