namespace TestTransaction
{
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
