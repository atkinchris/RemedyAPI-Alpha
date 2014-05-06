using System;
using System.Collections.Generic;

namespace RemedyAPI {
    public class Fields {

        private List<int> _fields = new List<int>();

        /// <summary>
        /// Add a single field to be included in the results.
        /// </summary>
        /// <param name="fieldID">Field ID</param>
        public void Add( int fieldID ) {
            _fields.Add( fieldID );
        }

        /// <summary>
        /// Add multiple fields to be included in the results.
        /// </summary>
        /// <param name="fieldIDs">Array of Field IDs</param>
        public void Add( int[] fieldIDs ) {
            foreach ( var fieldID in fieldIDs ) {
                Add( fieldID );
            }
        }

        /// <summary>
        /// Delete a single field from the list to be returned in results.
        /// </summary>
        /// <param name="fieldID">Field ID</param>
        public void Delete( int fieldID ) {
            if ( _fields.Contains( fieldID ) ) {
                _fields.Remove( fieldID );
            }
            else {
                throw new ArgumentException( string.Format( "Field {0} does not exist.", fieldID.ToString() ) );
            }
        }

        /// <summary>
        /// Delete multiple fields from the list to be returned in results.
        /// </summary>
        /// <param name="fieldIDs">Array of Field IDs</param>
        public void Delete( int[] fieldIDs ) {
            foreach ( var fieldID in fieldIDs ) {
                Delete( fieldID );
            }
        }

        /// <summary>
        /// Clear the list of fields to be returned in results.
        /// </summary>
        public void Clear() {
            _fields.Clear();
        }

        /// <summary>
        /// Get fields as integer array.
        /// </summary>
        /// <returns>Integer array</returns>
        public int[] ToArray() {
            return _fields.ToArray();
        }
    }
}
