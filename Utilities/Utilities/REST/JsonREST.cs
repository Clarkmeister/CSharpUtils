using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace Utilities.REST
{
    class JsonREST
    {
        public static T Post<T>(object sendObject, string host, string port, string path, string contentType = "application/text")
        {
            var endpoint = $"http://{host}:{port}/{path}";

            var body = JsonConvert.SerializeObject(sendObject, sendObject.GetType(),
                new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = contentType;
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = Convert.ToInt32(TimeSpan.FromMinutes(5).TotalMilliseconds);
            httpWebRequest.ReadWriteTimeout = Convert.ToInt32(TimeSpan.FromMinutes(5).TotalMilliseconds);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(body);
            }

            using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                var code = httpResponse.StatusCode;

                if (code != HttpStatusCode.OK)
                    throw new Exception(
                        $"Failed to get 200 StatusCode from http://{host}:{port}/{path}. Code: '{code}'. Description: '{httpResponse.StatusDescription}'");

                // ReSharper disable once AssignNullToNotNullAttribute
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(result,
                        new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                }
            }
        }

        public static T Get<T>(string host, string port, string path, string contentType = "application/text")
        {
            var endpoint = $"http://{host}:{port}/{path}";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);
            httpWebRequest.ContentType = contentType;
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = Convert.ToInt32(TimeSpan.FromMinutes(5).TotalMilliseconds);
            httpWebRequest.ReadWriteTimeout = Convert.ToInt32(TimeSpan.FromMinutes(5).TotalMilliseconds);

            using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                var code = httpResponse.StatusCode;

                if (code != HttpStatusCode.OK)
                    throw new Exception(
                        $"Failed to get 200 StatusCode from http://{host}:{port}/{path}. Code: '{code}'. Description: '{httpResponse.StatusDescription}'");

                // ReSharper disable once AssignNullToNotNullAttribute
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(result,
                        new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                }
            }
        }
    }
}
