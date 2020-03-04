using System.Collections.Generic;

namespace AWS.AmazonAuthSignatureV4
{
    public interface IAwsAuthSignaturCreator
    {
        Dictionary<string, string> GetHeaders();
    }
}