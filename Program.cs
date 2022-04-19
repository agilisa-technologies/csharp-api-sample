using RestSharp;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestTransaction
{
    class Program
    {
        private const string _baseUrl = "https://stage.agilpay.net/PaynetWebApi/";

        static async Task Main(string[] args)
        {
            string token = await GetTokenAsync(_baseUrl, "API-001", "Dynapay");
            Console.WriteLine("OAuth 2.0 token:\n" + token);

            var result2 = await Authorize(_baseUrl, token, "API-001");
            Console.WriteLine("Authorization Request:\n" + result2);

            Console.ReadLine();
        }

        static async Task<string> GetTokenAsync(string _baseUrl, string _clientId, string _clientSecret)
        {
            string result = null;
            try
            {
                var client = new RestClient(_baseUrl);

                var request = new RestRequest("oauth/token").AddParameter("grant_type", "client_credentials");
                request.AddParameter("client_id", _clientId);
                request.AddParameter("client_secret", _clientSecret);

                TokenResponse response = await client.PostAsync<TokenResponse>(request);

                result = $"{response.TokenType} {response!.AccessToken}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        static async Task<string> Authorize(string baseUrl, string token, string client_id)
        {
            try
            {
                var client = new RestClient(baseUrl);

                var request = new RestRequest("v6/Authorize");
                request.AddHeader("Content-Type", "application/json")
                        .AddHeader("SessionId", "ABCDEF")
                        .AddHeader("SiteId", client_id)
                        .AddHeader("Authorization", token);

                var authorizationRequest = new AuthorizationRequest()
                {
                    MerchantKey = "TEST-001",
                    AccountNumber = "5555555555554447",
                    ExpirationMonth = "01",
                    ExpirationYear = "23",
                    CustomerName = "Test User",
                    CustomerId = "4535435435",
                    AccountType = "1", // 1= CREDIT
                    CustomerEmail = "testuser@gmail.com",
                    ZipCode = "33167",
                    Amount = "10.21",
                    Currency = "840",
                    Tax = "0",
                    Invoice = "123465465", // Customer Account Number
                    Transaction_Detail = "payment information detail"
                };

                request.AddJsonBody(authorizationRequest);

                RestResponse response = await client.PostAsync(request);

                return response.Content;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

    }


    record TokenResponse
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; init; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }
    }

    internal class AuthorizationRequest
    {
        public string MerchantKey { get; set; }
        public string AccountNumber { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string CustomerName { get; set; }
        public string CustomerId { get; set; }
        public string AccountType { get; set; }
        public string CustomerEmail { get; set; }
        public string ZipCode { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string Tax { get; set; }
        public string Invoice { get; set; }
        public string Transaction_Detail { get; set; }

    }
}
