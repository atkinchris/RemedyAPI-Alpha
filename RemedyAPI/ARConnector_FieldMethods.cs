using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemedyAPI {
    public partial class ARConnector {
        private Fields fields = new Fields();

        public void AddField(int fieldID) {
            fields.Add(fieldID);
        }

    }
}
