using System;

namespace DotNetPoC.Functions.Shared;

public static class StreamConverter
{
  public static async Task<string> ToStringAsync(Stream stream)
  {
    using var reader = new StreamReader(stream);
    return await reader.ReadToEndAsync();
  }
}
