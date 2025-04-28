using System.Text.Json.Serialization;

namespace AuthyDecryptor.Model;

public class EncryptedToken
{
    [JsonPropertyName("key_derivation_iterations")]
    public int KeyDerivationIterations { get; set; }

    [JsonPropertyName("encrypted_seed")]
    public string? EncryptedSeed { get; set; }

    [JsonPropertyName("salt")]
    public string? Salt { get; set; }

    [JsonPropertyName("unique_iv")]
    public string? UniqueIv { get; set; }

    [JsonPropertyName("account_type")]
    public string? AccountType { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("issuer")]
    public string? Issuer { get; set; }

    [JsonPropertyName("digits")]
    public int Digits { get; set; }

    [JsonPropertyName("logo")]
    public string? Logo { get; set; }

    [JsonPropertyName("unique_id")]
    public string? UniqueId { get; set; }
}