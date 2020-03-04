using System.Collections.Generic;

namespace AWS.AmazonAuthSignatureV4
{
    public interface IAwsApiCaller<out TOutput>
    {
        TOutput Call(string payload, string requestUri, string httpMethod, Dictionary<string, string> headers);
    }
}