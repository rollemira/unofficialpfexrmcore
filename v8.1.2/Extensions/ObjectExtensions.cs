using System.IO;
using System.Runtime.Serialization.Json;

namespace Microsoft.Pfe.Xrm
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object o)
        {
            string result;
            using (var jsonStream = new MemoryStream())
            {
                //TODO: Maybe use Json.NET here? I was avoiding extra dependencies
                var serializer = new DataContractJsonSerializer(o.GetType());
                serializer.WriteObject(jsonStream, o);
                jsonStream.Position = 0;
                using (var reader = new StreamReader(jsonStream))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
    }
}
