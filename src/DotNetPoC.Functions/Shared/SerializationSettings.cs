using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace DotNetPoC.Functions.Shared;

public static class AppJsonSerializer
{
  private static JsonSerializerOptions? jsonSerializerOptions;
  private static JsonSerializerOptions JsonSettings(bool writeIndented = true)
  {

    jsonSerializerOptions ??= new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true,
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      ReadCommentHandling = JsonCommentHandling.Skip,
      WriteIndented = writeIndented
    };
    return jsonSerializerOptions;
  }
  public static string Serialize(object input, bool writeIndented = true)
  {
    return JsonSerializer.Serialize(input, JsonSettings(writeIndented));
  }
  public static DeSerializationResult<TOutput> Deserialize<TOutput>(string json)
  {
    try
    {
      var output = JsonSerializer.Deserialize<TOutput>(json, JsonSettings());
      if (output == null) return new DeSerializationResult<TOutput>(false, output);
      return new DeSerializationResult<TOutput>(true, output);
    }
    catch (JsonException ex)
    {
      return new DeSerializationResult<TOutput>(false, default(TOutput), ex);
    }

  }
}

public record DeSerializationResult<TOutput>(bool IsSuccess, TOutput? Value, Exception? Exception = null)
{
  public void ThrowExceptionIfFail(Func<Exception?, Exception> getException)
  {
    throw getException(Exception);
  }
}

