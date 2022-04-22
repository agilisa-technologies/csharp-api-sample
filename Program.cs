using RestSharp;
using System;
using System.Threading.Tasks;

namespace TestTransaction
{
    class Program
    {
        private const string _baseUrl = "https://stage.agilpay.net/PaynetWebApi/";

        static async Task Main(string[] args)
        {
            Console.Write("Client_id [API-001]:");
            var client_id = Console.ReadLine();
            client_id = string.IsNullOrEmpty(client_id) ? "API-001": client_id;

            Console.Write("Secret [Dynapay]:");
            var secret = Console.ReadLine();
            secret = string.IsNullOrEmpty(secret)? "Dynapay": secret;

            string token = await GetTokenAsync(_baseUrl, client_id, secret);
            Console.WriteLine("OAuth 2.0 token:\n" + token);


            Console.Write("Merchant Key [TEST-001]:");
            var merchant_key = Console.ReadLine();
            merchant_key = string.IsNullOrEmpty(merchant_key) ? "TEST-001" : merchant_key;

            Console.Write("Customer Account [123456]:");
            var customer_id = Console.ReadLine();
            customer_id = string.IsNullOrEmpty(customer_id) ? "123456" : customer_id;

            var resultBalance = await GetBalance(_baseUrl, token, client_id, merchant_key, customer_id);
            Console.WriteLine("Balance Result:\n" + resultBalance);

            Console.Write("\nPayment Amount:");
            var amount = Console.ReadLine();
            amount = string.IsNullOrEmpty(amount) ? "1.02" : amount;

            var resultPayment = await Authorize(_baseUrl, token, client_id, merchant_key, customer_id, amount);
            Console.WriteLine("Authorization Result:\n" + resultPayment);

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

        static async Task<string> Authorize(string baseUrl, string token, string client_id, string merchant_key, string customer_id, string amount)
        {
            try
            {
                var client = new RestClient(baseUrl);

                var request = new RestRequest("Payment6/Autorize");
                request.AddHeader("Content-Type", "application/json")
                        .AddHeader("SessionId", "ABCDEF")
                        .AddHeader("SiteId", client_id)
                        .AddHeader("Authorization", token);

                var authorizationRequest = new AuthorizationRequest()
                {
                    MerchantKey = merchant_key,
                    AccountNumber = "5555555555554447",
                    ExpirationMonth = "01",
                    ExpirationYear = "23",
                    CustomerName = "Test User",
                    CustomerId = customer_id,
                    AccountType = "1", // 1= CREDIT
                    CustomerEmail = "testuser@gmail.com",
                    ZipCode = "33167",
                    Amount = amount,
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

        static async Task<string> GetBalance(string baseUrl, string token, string client_id, string merchant_key, string customer_id)
        {
            try
            {
                var client = new RestClient(baseUrl);
                var request = new RestRequest("Payment6/GetBalance");
                request.AddHeader("Content-Type", "application/json")
                        .AddHeader("SessionId", "ABCDEF")
                        .AddHeader("SiteId", client_id)
                        .AddHeader("Authorization", token);

                var balanceRequest = new BalanceRequest()
                {
                    MerchantKey = merchant_key,
                    CustomerId = customer_id,
                };

                request.AddJsonBody(balanceRequest);

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
}
