using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AWS.AmazonAuthSignatureV4
{
    public class AwsAuthSignaturCreator : IAwsAuthSignaturCreator
    {
        private string AwsAccessKey { get; set; }
        private string AwsSecretKey { get; set; }
        private string Path { get; set; }
        private string Region { get; set; }
        private string Service { get; set; }
        private string HttpMethodName { get; set; }
        private Dictionary<string, string> Headers { get; set; }
        private string Payload { get; set; }
        private string HmacAlgorithm { get; set; }
        private string Aws4Request { get; set; }
        private string SignedHeaders { get; set; }
        private string XAmzDate { get; set; }
        private string CurrentDate { get; set; }

        private const string K_HEX_ALPHABET = "0123456789ABCDEF";

        public AwsAuthSignaturCreator(string awsAccessKey, string awsSecretKey, string path, string region, string service,
            string httpMethodName, Dictionary<string, string> headers, string payload = null)
        {
            if (string.IsNullOrWhiteSpace(awsAccessKey))
                throw new ArgumentNullException(nameof(awsAccessKey));
            if (string.IsNullOrWhiteSpace(awsSecretKey))
                throw new ArgumentNullException(nameof(awsSecretKey));
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));
            if (string.IsNullOrWhiteSpace(region))
                throw new ArgumentNullException(nameof(region));
            if (string.IsNullOrWhiteSpace(service))
                throw new ArgumentNullException(nameof(service));
            if (string.IsNullOrWhiteSpace(httpMethodName))
                throw new ArgumentNullException(nameof(httpMethodName));            

            AwsAccessKey = awsAccessKey;
            AwsSecretKey = awsSecretKey;
            Path = path;
            Region = region;
            Service = service;
            HttpMethodName = httpMethodName;
            Headers = headers == null ? Headers = new Dictionary<string, string>() : headers;
            Payload = payload;
            HmacAlgorithm = "AWS4-HMAC-SHA256";
            Aws4Request = "aws4_request";

            XAmzDate = GetTimeStamp();
            CurrentDate = GetDate();
        }
       
        public Dictionary<string, string> GetHeaders()
        {
            Headers.Remove("x-amz-date");
            Headers.Remove("Authorization");
            Headers.Add("x-amz-date", XAmzDate);            
            var canonicalURL = PrepareCanonicalRequest();            
            var stringToSign = PrepareStringToSign(canonicalURL);            
            var signature = calculateSignature(stringToSign);            
            if (signature != null)
            {
                Headers.Add("Authorization", BuildAuthorizationString(signature));
                return Headers;
            }
            else
            {
                return null;
            }
        }

        private string PrepareCanonicalRequest()
        {
            var canonicalUrl = new StringBuilder();
            canonicalUrl.Append(HttpMethodName.ToUpper()).Append("\n");
            canonicalUrl.Append(Path).Append("\n").Append("\n");
            var signedHeaderBuilder = new StringBuilder();
            var sortedHeaders = Headers.OrderBy(h => h.Key);
            if (sortedHeaders != null)
            {
                foreach (var entrySet in sortedHeaders)
                {
                    var key = entrySet.Key;
                    var value = entrySet.Value;
                    signedHeaderBuilder.Append(key.ToLower().Trim()).Append(";");
                    canonicalUrl.Append(key.ToLower().Trim()).Append(":").Append(value.Trim()).Append("\n");
                }
                canonicalUrl.Append("\n");
            }
            else
            {
                canonicalUrl.Append("\n");
            }

            SignedHeaders = signedHeaderBuilder.ToString().Substring(0, signedHeaderBuilder.Length - 1);
            canonicalUrl.Append(SignedHeaders).Append("\n");

            Payload = Payload == null ? "" : Payload;
            canonicalUrl.Append(ToHex(Payload));

            return canonicalUrl.ToString();
        }

        private string PrepareStringToSign(string canonicalUrl)
        {
            var stringToSign = "";
            stringToSign = HmacAlgorithm + "\n";
            stringToSign += XAmzDate + "\n";
            stringToSign += CurrentDate + "/" + Region + "/" + Service + "/" + Aws4Request + "\n";
            stringToSign += ToHex(canonicalUrl);
            return stringToSign;
        }

        private string calculateSignature(string stringToSign)
        {
            var signatureKey = GetSignatureKey(AwsSecretKey, CurrentDate, Region, Service);
            var signature = HmacSha256(signatureKey, stringToSign);
            return BytesToHex(signature);
        }

        private string BuildAuthorizationString(string signature)
        {
            return HmacAlgorithm + " "
                    + "Credential=" + AwsAccessKey + "/" + GetDate() + "/" + Region + "/" + Service + "/" + Aws4Request + ", "
                    + "SignedHeaders=" + SignedHeaders + ", "
                    + "Signature=" + signature;
        }

        private string ToHex(string data)
        {
            var sha1 = SHA256.Create();
            var inputBytes = Encoding.UTF8.GetBytes(data);
            var outputBytes = sha1.ComputeHash(inputBytes);
            return BytesToHex(outputBytes);
        }

        private byte[] HmacSha256(byte[] key, string data)
        {
            var algorithm = "HmacSHA256";
            var kha = KeyedHashAlgorithm.Create(algorithm);
            kha.Key = key;

            return kha.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        private byte[] GetSignatureKey(string key, string date, string regionName, string serviceName)
        {
            var kSecret = Encoding.UTF8.GetBytes("AWS4" + key);
            var kDate = HmacSha256(kSecret, date);
            var kRegion = HmacSha256(kDate, regionName);
            var kService = HmacSha256(kRegion, serviceName);
            var kSigning = HmacSha256(kService, Aws4Request);
            return kSigning;
        }

        public static string BytesToHex(byte[] bytes)
        {
            var result = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                result.Append(K_HEX_ALPHABET[(int)(b >> 4)]);
                result.Append(K_HEX_ALPHABET[(int)(b & 0xF)]);
            }
            return result.ToString().ToLower();
        }

        private string GetTimeStamp()
        {
            var dateTime = DateTime.UtcNow;
            return dateTime.ToString("yyyyMMdd'T'HHmmss'Z'");
        }

        private string GetDate()
        {
            var dateTime = DateTime.UtcNow;
            return dateTime.ToString("yyyyMMdd");
        }
    }
}
