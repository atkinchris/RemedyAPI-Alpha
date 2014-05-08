using System.Collections.Generic;

namespace RemedyAPI {
    public partial class ARConnector {
        public Dictionary<string, int> GetResultsCount() {
            return queries.GetResultsCount();
        }

        public string GetResultsString() {
            return queries.GetResultsString();
        }
    }
}
