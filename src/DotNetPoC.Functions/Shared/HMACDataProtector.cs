using System.Security.Cryptography;
using System.Text;

namespace DotNetPoC.Functions.Shared;

public class HMACDataProtector(string securityKey, char separator = '|')
{
  public string GetToken(IEnumerable<string> tokenValues)
  {
    var tokenValue = string.Join(separator, tokenValues);
    var tokenHash = GenerateHash(tokenValue);
    var tokenValueWithHash = string.Join(separator, tokenValue, tokenHash);
    return Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenValueWithHash));
  }
  public ValueFromTokenResult GetValueFromToken(string token)
  {
    var tokenBytes = Convert.FromBase64String(token);
    var tokenValueWithHash = Encoding.UTF8.GetString(tokenBytes);
    var tokenParts = tokenValueWithHash.Split(separator);
    if (tokenParts.Length <= 1) return new ValueFromTokenResult(invalidSignature: true);

    var tokenHash = tokenParts[^1];
    var tokenValues = new string[tokenParts.Length - 1];
    Array.Copy(tokenParts, tokenValues, tokenValues.Length);
    var tokenValue = string.Join(separator, tokenValues);
    var newCalculateHash = GenerateHash(tokenValue);
    if (tokenHash != newCalculateHash) return new ValueFromTokenResult(isAltered: true);

    return new ValueFromTokenResult(values: tokenValues);
  }
  private string GenerateHash(string value)
  {
    var securityKeyBytes = Encoding.UTF8.GetBytes(securityKey);
    using var hmac256 = new HMACSHA256(securityKeyBytes);
    var tokenValueBytes = Encoding.UTF8.GetBytes(value);
    var tokenHashBytes = hmac256.ComputeHash(tokenValueBytes);
    return Convert.ToBase64String(tokenHashBytes);
  }

}

