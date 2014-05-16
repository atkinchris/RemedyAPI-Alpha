using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace RemedyAPI_Example {
    static class JSON {

        static public void WritetoFile(object obj, string filename) {
            using ( FileStream fs = File.Open( filename, FileMode.OpenOrCreate ) )
            using ( StreamWriter sw = new StreamWriter( fs ) )
            using ( JsonWriter jw = new JsonTextWriter( sw ) ) {
                jw.Formatting = Formatting.Indented;
                JsonSerializer serializer = new JsonSerializer() {
                    NullValueHandling = NullValueHandling.Ignore
                };
                serializer.Serialize( jw, obj );
            }
        }
    }
}
