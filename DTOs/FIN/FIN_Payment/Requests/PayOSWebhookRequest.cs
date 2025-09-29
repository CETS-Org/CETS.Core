using System.Text.Json.Serialization;

namespace DTOs.FIN.FIN_Payment.Requests
{
    public class PayOSWebhookRequest
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = null!;

        [JsonPropertyName("desc")]
        public string Description { get; set; } = null!;

        [JsonPropertyName("data")]
        public PayOSWebhookData Data { get; set; } = null!;

        [JsonPropertyName("signature")]
        public string Signature { get; set; } = null!;
    }

    public class PayOSWebhookData
    {
        [JsonPropertyName("orderCode")]
        public long OrderCode { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;

        [JsonPropertyName("accountNumber")]
        public string AccountNumber { get; set; } = null!;

        [JsonPropertyName("reference")]
        public string Reference { get; set; } = null!;

        [JsonPropertyName("transactionDateTime")]
        public long TransactionDateTime { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = null!;

        [JsonPropertyName("paymentLinkId")]
        public string PaymentLinkId { get; set; } = null!;

        [JsonPropertyName("code")]
        public string Code { get; set; } = null!;

        [JsonPropertyName("desc")]
        public string Desc { get; set; } = null!;

        [JsonPropertyName("counterAccountBankId")]
        public string CounterAccountBankId { get; set; } = null!;

        [JsonPropertyName("virtualAccountName")]
        public string VirtualAccountName { get; set; } = null!;

        [JsonPropertyName("virtualAccountNumber")]
        public string VirtualAccountNumber { get; set; } = null!;

        [JsonPropertyName("counterAccountBankName")]
        public string CounterAccountBankName { get; set; } = null!;

        [JsonPropertyName("counterAccountName")]
        public string CounterAccountName { get; set; } = null!;

        [JsonPropertyName("counterAccountNumber")]
        public string CounterAccountNumber { get; set; } = null!;
    }
}

