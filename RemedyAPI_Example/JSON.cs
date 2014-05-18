using Newtonsoft.Json;
using System.IO;

namespace RemedyAPI_Example {
    static class Json {

        static public void WritetoFile(object obj, string filename) {
            using ( var fs = File.Open( filename, FileMode.OpenOrCreate ) )
            using ( var sw = new StreamWriter( fs ) )
            using ( JsonWriter jw = new JsonTextWriter( sw ) ) {
                jw.Formatting = Formatting.Indented;
                var serializer = new JsonSerializer {
                    NullValueHandling = NullValueHandling.Ignore
                };
                serializer.Serialize( jw, obj );
            }
        }
    }
}
