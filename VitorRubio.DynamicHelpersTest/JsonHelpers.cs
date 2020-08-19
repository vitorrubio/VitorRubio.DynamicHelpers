using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace VitorRubio.DynamicHelpersTest
{
    public static class JsonHelpers
    {
        public static string ToJsonStringUsingNewtonsoftJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MaxDepth = 2,

            });
        }

        public static string ToJsonStringUsingDataContractJsonSerialyzer(object obj)
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer js = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            MemoryStream msObj = new MemoryStream();
            js.WriteObject(msObj, obj);
            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);
            string json = sr.ReadToEnd();
            sr.Close();
            msObj.Close();
            return json;
        }

        public static string ToJsonStringUsingJavaScriptJsonSerializer(object obj)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(obj);
            return jsonData;
        }
    }
}
