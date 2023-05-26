using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SceneryLauncher
{
    internal class Auth
    {

        public static string GetCredentialsToken()
        {
            RestClient RestClient = new RestClient("https://account-public-service-prod03.ol.epicgames.com");
            RestRequest RestRequest = new RestRequest("/account/api/oauth/token", Method.Post);
            RestRequest.AddHeader("Authorization", "Basic OThmN2U0MmMyZTNhNGY4NmE3NGViNDNmYmI0MWVkMzk6MGEyNDQ5YTItMDAxYS00NTFlLWFmZWMtM2U4MTI5MDFjNGQ3");
            RestRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            RestRequest.AddParameter("grant_type", "client_credentials");
            RestResponse RestResponse = RestClient.Execute(RestRequest);

            var CredentialsToken = JObject.Parse(RestResponse.Content)["access_token"].ToString();
            return CredentialsToken;
        }

        public static string GetAccessToken(string CredentialsToken)
        {
            RestClient RestClient = new RestClient("https://account-public-service-prod03.ol.epicgames.com");
            RestRequest RestRequest = new RestRequest("/account/api/oauth/deviceAuthorization", Method.Post);
            RestRequest.AddHeader("Authorization", $"Bearer {CredentialsToken}");
            RestRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            RestResponse RestResponse = RestClient.Execute(RestRequest);

            var VerificationUri = JObject.Parse(RestResponse.Content)["verification_uri_complete"].ToString();
            var DeviceCode = JObject.Parse(RestResponse.Content)["device_code"].ToString();

            Process.Start(VerificationUri);

            for (; ; )
            {
                RestClient = new RestClient("https://account-public-service-prod03.ol.epicgames.com");
                RestRequest = new RestRequest("/account/api/oauth/token", Method.Post);
                RestRequest.AddHeader("Authorization", "Basic OThmN2U0MmMyZTNhNGY4NmE3NGViNDNmYmI0MWVkMzk6MGEyNDQ5YTItMDAxYS00NTFlLWFmZWMtM2U4MTI5MDFjNGQ3");
                RestRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                RestRequest.AddParameter("grant_type", "device_code");
                RestRequest.AddParameter("device_code", DeviceCode);
                RestResponse = RestClient.Execute(RestRequest);

                if (RestResponse.Content.Contains("access_token"))
                {
                    var AccessToken = JObject.Parse(RestResponse.Content)["access_token"].ToString();
                    return AccessToken;
                }
                if (RestResponse.Content.Contains("errors.com.epicgames.not_found"))
                {
                    Environment.Exit(0);
                }
                if (RestResponse.Content.Contains("errors.com.epicgames.account.authorization_pending"))
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public static string GetExchangeCode(string AccessToken)
        {
            RestClient RestClient = new RestClient("https://account-public-service-prod03.ol.epicgames.com");
            RestRequest RestRequest = new RestRequest("/account/api/oauth/exchange", Method.Get);
            RestRequest.AddHeader("Authorization", $"Bearer {AccessToken}");
            RestResponse RestResponse = RestClient.Execute(RestRequest);

            var ExchangeCode = JObject.Parse(RestResponse.Content)["code"].ToString();
            return ExchangeCode;
        }
    }
}
