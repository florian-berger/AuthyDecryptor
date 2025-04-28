using System.Text.Json.Serialization;

namespace AuthyDecryptor.Model;

public class DecryptedToken
{
    [JsonPropertyName("account_type")]
    public string? AccountType { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("issuer")]
    public string? Issuer { get; set; }

    [JsonPropertyName("decrypted_seed")]
    public string? DecryptedSeed { get; set; }

    [JsonPropertyName("digits")]
    public int Digits { get; set; }

    [JsonPropertyName("logo")]
    public string? Logo { get; set; }

    [JsonPropertyName("unique_id")]
    public string? UniqueId { get; set; }
}