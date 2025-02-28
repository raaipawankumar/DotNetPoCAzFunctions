using System;
using Microsoft.Azure.Functions.Worker;

namespace DotNetPoC.Functions.Models;

public class CosmosDBResponse
{
  [CosmosDBOutput(databaseName: "%CosmosDB:Database%", containerName: "%CosmosDB:Container%", Connection = "CosmosDB")]
  public string Document { get; set; } = string.Empty;
}
