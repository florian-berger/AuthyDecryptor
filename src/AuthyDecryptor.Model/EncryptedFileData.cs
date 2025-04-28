using System.Text.Json.Serialization;

namespace AuthyDecryptor.Model;

public class EncryptedFileData
{
    [JsonPropertyName("authenticator_tokens")]
    public List<EncryptedToken> AuthenticatorTokens { get; set; }
}