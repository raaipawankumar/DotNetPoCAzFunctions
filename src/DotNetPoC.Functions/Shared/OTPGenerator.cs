using System.Text;

namespace DotNetPoC.Functions.Shared;

public static class OTPGenerator
{
  private readonly static string OTPAlphaNumericChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
  private readonly static string OTPNumericChars = "0123456789";
  public static string GenerateNumeric(byte size)
  {
    if (size < 1) throw new ArgumentException($"{nameof(size)} can not be less than 1. Value is ${size}");
    var otpBuilder = new StringBuilder();
    var random = new Random();
    while (size > 0)
    {
      var randomIndex = random.Next(OTPNumericChars.Length);
      otpBuilder.Append(OTPNumericChars[randomIndex]);
      size--;
    }
    return otpBuilder.ToString();
  }
  public static string GenerateAlphaNumeric(byte size)
  {
    if (size < 1) throw new ArgumentException($"{nameof(size)} can not be less than 1. Value is ${size}");
    var otpBuilder = new StringBuilder();
    var random = new Random();
    while (size > 0)
    {
      var randomIndex = random.Next(OTPAlphaNumericChars.Length);
      otpBuilder.Append(OTPAlphaNumericChars[randomIndex]);
      size--;
    }
    return otpBuilder.ToString();
  }

}
