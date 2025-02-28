using System;

namespace DotNetPoC.Functions.Models;

public record WebPageRequest
{
  private string generatedFileName = Guid.NewGuid().ToString();
  public string? Url { get; set; }
  public string GeneratedFileName
  {
    get
    {
      return generatedFileName;
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
