using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AWS.AmazonAuthSignatureV4
{
    public class AwsApiCaller : IAwsApiCaller<Task<string>>
    {
        public async Task<string> Call(string payload, string requestUri, string httpMethod, Dictionary<string, string> headers)
        {

            switch (httpMethod.ToUpper())
            {
                case "POST":
                case "GET": break;
                default: throw new InvalidOperationException("[AwsApiCaller] Only POST and GET methods are supported");
            }

            string contentString = "";
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(requestUri);

                request.Method = httpMethod.ToUpper();
                request.Headers.Add("Content-Encoding", "amz-1.0");
                request.Headers.Add("Authorization", headers["Authorization"]);
                request.Headers.Add("x-amz-target", headers["x-amz-target"]);
                request.Headers.Add("x-amz-date", headers["x-amz-date"]);
                request.ContentType = "application/json; charset=utf-8";
                var responseContent = new MemoryStream();

                if (httpMethod.ToUpper() == "POST")
                {
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(payload);
                    }
                }

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        await stream.CopyToAsync(responseContent);
                    }
                }

                contentString = Encoding.ASCII.GetString(responseContent.ToArray());
                return contentString;
            }
            catch (HttpRequestException ex)
            {
                StringBuilder message = new StringBuilder();
                message.Append(ex.Message);
                message.AppendFormat(" requestUri: {0}", requestUri);
                message.AppendFormat(" httpResponseString: {0}", contentString);
                throw new HttpRequestException(message.ToString(), ex);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
