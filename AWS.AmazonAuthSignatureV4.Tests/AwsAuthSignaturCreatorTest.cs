using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWS.AmazonAuthSignatureV4.Tests
{
    [TestClass]
    public class AwsAuthSignaturCreatorTest
    {
        public AwsAuthSignaturCreator Target { get; set; }
        public IConfiguration AppConfig { get; set; }

        [TestInitialize]
        public void Init()
        {            
            AppConfig = new ConfigurationBuilder()
                      .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                      .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                      .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AwsAuthSignaturCreator_GetHeaders_AwsAccessKeyIsEmpty_ThrowsArgumentNullException()
        {

            Target = new AwsAuthSignaturCreator(
                "",
                AppConfig["AwsSecretKey"],
                AppConfig["Path"],
                AppConfig["Region"],
                AppConfig["Service"],
                "POST", null);
            Target.GetHeaders();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AwsAuthSignaturCreator_GetHeaders_AwsAccessKeyIsNull_ThrowsArgumentNullException()
        {

            Target = new AwsAuthSignaturCreator(
                null,
                AppConfig["AwsSecretKey"],
                AppConfig["Path"],
                AppConfig["Region"],
                AppConfig["Service"],
                "POST", null);
            Target.GetHeaders();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AwsAuthSignaturCreator_GetHeaders_AwsSecretKeyIsEmpty_ThrowsArgumentNullException()
        {

            Target = new AwsAuthSignaturCreator(
                AppConfig["AwsAccessKey"],
                "",
                AppConfig["Path"],
                AppConfig["Region"],
                AppConfig["Service"],
                "POST", null);
            Target.GetHeaders();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AwsAuthSignaturCreator_GetHeaders_AwsSecretKeyIsNull_ThrowsArgumentNullException()
        {

            Target = new AwsAuthSignaturCreator(
                AppConfig["AwsAccessKey"],
                null,
                AppConfig["Path"],
                AppConfig["Region"],
                AppConfig["Service"],
                "POST", null);
            Target.GetHeaders();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AwsAuthSignaturCreator_GetHeaders_PathIsEmpty_ThrowsArgumentNullException()
        {

            Target = new AwsAuthSignaturCreator(
                AppConfig["AwsAccessKey"],
                AppConfig["AwsSecretKey"],
                "",
                AppConfig["Region"],
                AppConfig["Service"],
                "POST", null);
            Target.GetHeaders();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AwsAuthSignaturCreator_GetHeaders_PathIsNull_ThrowsArgumentNullException()
        {

            Target = new AwsAuthSignaturCreator(
                AppConfig["AwsAccessKey"],
                AppConfig["AwsSecretKey"],
                null,
                AppConfig["Region"],
                AppConfig["Service"],
                "POST", null);
            Target.GetHeaders();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AwsAuthSignaturCreator_GetHeaders_RegionIsEmpty_ThrowsArgumentNullException()
        {

            Target = new AwsAuthSignaturCreator(
                AppConfig["AwsAccessKey"],
                AppConfig["AwsSecretKey"],
                AppConfig["Path"],
                "",
                AppConfig["Service"],
                "POST", null);
            Target.GetHeaders();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AwsAuthSignaturCreator_GetHeaders_RegionIsNull_ThrowsArgumentNullException()
        {

            Target = new AwsAuthSignaturCreator(
                AppConfig["AwsAccessKey"],
                AppConfig["AwsSecretKey"],
                AppConfig["Path"],
                null,
                AppConfig["Service"],
                "POST", null);
            Target.GetHeaders();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AwsAuthSignaturCreator_GetHeaders_ServiceIsEmpty_ThrowsArgumentNullException()
        {

            Target = new AwsAuthSignaturCreator(
                AppConfig["AwsAccessKey"],
                AppConfig["AwsSecretKey"],
                AppConfig["Path"],
                AppConfig["Region"],
                "",
                "POST", null);
            Target.GetHeaders();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AwsAuthSignaturCreator_GetHeaders_ServiceIsNull_ThrowsArgumentNullException()
        {

            Target = new AwsAuthSignaturCreator(
                AppConfig["AwsAccessKey"],
                AppConfig["AwsSecretKey"],
                AppConfig["Path"],
                AppConfig["Region"],
                null,
                "POST", null);
            Target.GetHeaders();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AwsAuthSignaturCreator_GetHeaders_HttpMethodNameIsEmpty_ThrowsArgumentNullException()
        {

            Target = new AwsAuthSignaturCreator(
                AppConfig["AwsAccessKey"],
                AppConfig["AwsSecretKey"],
                AppConfig["Path"],
                AppConfig["Region"],
                AppConfig["Service"],
                "", null);
            Target.GetHeaders();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AwsAuthSignaturCreator_GetHeaders_HttpMethodNameeIsNull_ThrowsArgumentNullException()
        {

            Target = new AwsAuthSignaturCreator(
                AppConfig["AwsAccessKey"],
                AppConfig["AwsSecretKey"],
                AppConfig["Path"],
                AppConfig["Region"],
                AppConfig["Service"],
                null, null);
            Target.GetHeaders();
        }

        [TestMethod]        
        public void AwsAuthSignaturCreator_GetHeaders_VerifyHeaders_CountShouldBe2()
        {
            Target = new AwsAuthSignaturCreator(
                AppConfig["AwsAccessKey"],
                AppConfig["AwsSecretKey"],
                AppConfig["Path"],
                AppConfig["Region"],
                AppConfig["Service"],
                "POST", null);
            Assert.AreEqual(2, Target.GetHeaders().Count);
        }

        [TestMethod]
        public void AwsAuthSignaturCreator_GetHeaders_VerifyHeaders_VerifyXAmzDate()
        {
            Target = new AwsAuthSignaturCreator(
                AppConfig["AwsAccessKey"],
                AppConfig["AwsSecretKey"],
                AppConfig["Path"],
                AppConfig["Region"],
                AppConfig["Service"],
                "POST", null);
            Assert.IsTrue(Target.GetHeaders().ContainsKey("x-amz-date"));
            Assert.IsNotNull(Target.GetHeaders()["x-amz-date"]);
        }

        [TestMethod]
        public void AwsAuthSignaturCreator_GetHeaders_VerifyHeaders_VerifyAuthorization()
        {
            Target = new AwsAuthSignaturCreator(
                AppConfig["AwsAccessKey"],
                AppConfig["AwsSecretKey"],
                AppConfig["Path"],
                AppConfig["Region"],
                AppConfig["Service"],
                "POST", null);
            Assert.IsTrue(Target.GetHeaders().ContainsKey("Authorization"));
            Assert.IsNotNull(Target.GetHeaders()["Authorization"]);
        }

        [TestMethod]
        public void AwsAuthSignaturCreator_GetHeaders_VerifyHeadersCountIfHeadresWereNotEmpty_ReturnsCountAs4()
        {
            Target = new AwsAuthSignaturCreator(
                AppConfig["AwsAccessKey"],
                AppConfig["AwsSecretKey"],
                AppConfig["Path"],
                AppConfig["Region"],
                AppConfig["Service"],
                "POST", new Dictionary<string, string>()
                {
                    {"test1","test1" },
                    {"test2","test2" }
                });
            Assert.AreEqual(4, Target.GetHeaders().Count);
            
            Assert.IsNotNull(Target.GetHeaders()["test1"]);
            Assert.IsNotNull(Target.GetHeaders()["test2"]);

            Assert.AreEqual("test2", Target.GetHeaders()["test2"]);
            Assert.AreEqual("test1", Target.GetHeaders()["test1"]);

            Assert.IsTrue(Target.GetHeaders().ContainsKey("x-amz-date"));
            Assert.IsNotNull(Target.GetHeaders()["x-amz-date"]);

            Assert.IsTrue(Target.GetHeaders().ContainsKey("Authorization"));
            Assert.IsNotNull(Target.GetHeaders()["Authorization"]);
        }

        [TestMethod]
        public void AwsAuthSignaturCreator_GetHeaders_VerifyAuthorizationFormat_ReturnsCorrectFormat()
        {
            Target = new AwsAuthSignaturCreator(
                AppConfig["AwsAccessKey"],
                AppConfig["AwsSecretKey"],
                AppConfig["Path"],
                AppConfig["Region"],
                AppConfig["Service"],
                "POST", null);
            var authorization = Target.GetHeaders()["Authorization"];            
            Assert.IsTrue(authorization.StartsWith("AWS4-HMAC-SHA256 "));            
            var toComp = $"Credential={AppConfig["AwsAccessKey"]}/{DateTime.Now.ToString("yyyyMMdd")}/{AppConfig["Region"]}/{AppConfig["Service"]}/aws4_request,";
            Assert.IsTrue(authorization.Split(' ')[1] == toComp);

            Assert.IsTrue(authorization.Split(' ')[2] == "SignedHeaders=x-amz-date,");
            Assert.IsTrue(authorization.Split(' ')[3].StartsWith("Signature="));
        }
    }
}
