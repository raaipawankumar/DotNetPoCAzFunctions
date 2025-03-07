using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DotNetPoC.Functions.Shared;

public static class JsonConvertSettings
{
  public static JsonSerializerSettings DefaultSerializerSettings
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
  public static JsonSerializerSettings UnIndentedSerializerSettings
  {
    get
    {
      return new JsonSerializerSettings
      {
        Formatting = Formatting.None,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
      };
    }
  }
}
