﻿using AWS.AmazonAuthSignatureV4;
using System;
using System.Collections.Generic;

namespace AWSSignatureSample1
{
    class Program
    {
        private static string _awsAccessKeyId { get; set; } = "<AWS_AccessKeyId>";
        private static string _awsSecretKey { get; set; } = "<AWS_SecretKey>";
        private static string _locale { get; set; } = ".com";
        private static string _associateTag { get; set; } = "sampletag";
        private const string K_RESOURCES = "\"CustomerReviews.Count\",\"CustomerReviews.StarRating\",\"Images.Primary.Small\",\"Images.Primary.Medium\",\"Images.Primary.Large\",\"Images.Variants.Small\",\"Images.Variants.Medium\",\"Images.Variants.Large\",\"ItemInfo.ByLineInfo\",\"ItemInfo.ContentInfo\",\"ItemInfo.ContentRating\",\"ItemInfo.Classifications\",\"ItemInfo.ExternalIds\",\"ItemInfo.Features\",\"ItemInfo.ManufactureInfo\",\"ItemInfo.ProductInfo\",\"ItemInfo.TechnicalInfo\",\"ItemInfo.Title\",\"ItemInfo.TradeInInfo\",\"Offers.Listings.Availability.MaxOrderQuantity\",\"Offers.Listings.Availability.Message\",\"Offers.Listings.Availability.MinOrderQuantity\",\"Offers.Listings.Availability.Type\",\"Offers.Listings.Condition\",\"Offers.Listings.Condition.SubCondition\",\"Offers.Listings.DeliveryInfo.IsAmazonFulfilled\",\"Offers.Listings.DeliveryInfo.IsFreeShippingEligible\",\"Offers.Listings.DeliveryInfo.IsPrimeEligible\",\"Offers.Listings.DeliveryInfo.ShippingCharges\",\"Offers.Listings.IsBuyBoxWinner\",\"Offers.Listings.LoyaltyPoints.Points\",\"Offers.Listings.MerchantInfo\",\"Offers.Listings.Price\",\"Offers.Listings.ProgramEligibility.IsPrimeExclusive\",\"Offers.Listings.ProgramEligibility.IsPrimePantry\",\"Offers.Listings.Promotions\",\"Offers.Listings.SavingBasis\"";

        static void Main(string[] args)
        {
            var productId = "B07DPPY2MW";
            var initHeaders = new Dictionary<string, string>()
            {
                {"content-encoding", "amz-1.0" },
                {"content-type", "application/json; charset=utf-8" },
                {"host", "webservices.amazon.com" },
                {"x-amz-target", "com.amazon.paapi5.v1.ProductAdvertisingAPIv1.GetItems" }
            };
            var payload = "{\"Marketplace\":\"www.amazon" + _locale + "\",\"PartnerType\":\"Associates\",\"PartnerTag\":\"" + _associateTag + "\",\"ItemIds\":[\""
                + productId + "\"],\"Resources\":[" + K_RESOURCES + "]}";

            var creator = new AwsAuthSignaturCreator(_awsAccessKeyId, _awsSecretKey, "/paapi5/getitems", "us-east-1", "ProductAdvertisingAPI", "POST", initHeaders, payload);
            var headers = creator.GetHeaders();
            var requestUri = "https://webservices.amazon.com/paapi5/searchitems";
            var caller = new AwsApiCaller();
            string contentString = caller.Call(payload, requestUri, "POST", headers).Result;
        }
    }
}
