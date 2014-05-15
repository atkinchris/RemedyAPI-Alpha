using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RemedyAPI_Test {
    class Program {

        static void Main()
        {

            var list = new Dictionary<long, int> {{DateTime.Now.ToJavaScriptTimestamp(), 1}, {DateTime.Now.AddDays(1).ToJavaScriptTimestamp(), 3}};
            var json = JsonConvert.SerializeObject( list );

            Console.Write(json);
            Console.ReadLine();

        }
    }
}
