using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotNetPoC.Functions.Models;

public record PageToPdfRequest
{
  private string? generatedFileName;
  public string? Url { get; set; }
  public string GeneratedFileName
  {
    get
    {
      if (string.IsNullOrWhiteSpace(generatedFileName))
      {
        return Guid.NewGuid().ToString();
      }
      return $"{generatedFileName}-{DateTime.UtcNow.ToString("yyyymmddhhMMss")}";
    }
    set
    {
      if (!string.IsNullOrWhiteSpace(value))
      {
        generatedFileName = value;
      }

    }
  }

}
