using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DotNetPoC.Functions.Models;

public static class AppJsonSerializer
{
  public static JsonSerializerSettings Default
  {
    get
    {
      return new JsonSerializerSettings
      {
        Formatting = Formatting.Indented,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      };
    }
  }

}
