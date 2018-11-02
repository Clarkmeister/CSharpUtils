using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace QCallerWebService.Utilities.Json
{
    public static class Extensions
    {
        public static T ToObject<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static T ToObject<T>(this Stream stream)
        {
            var reader = new StreamReader(stream);
            var body = reader.ReadToEnd();
            return ToObject<T>(body);
        }

        public static string ToJsonString(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                });
        }

        public static Stream ToStream(this object obj)
        {
            return obj.ToJsonString().ToStream();
        }

        public static Stream ToStream(this string str)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(str));
        }

        //public static int? ToNullableInt(this string s)
        //{
        //    if (int.TryParse(s, out var i)) return i;
        //    return null;
        //}
    }
}

