using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;

namespace DotNetPoC.Functions;

public class AddAuditTrailToDbFunction()
{
  [Function(nameof(AddAuditTrailToDbFunction))]
  [CosmosDBOutput(databaseName: "%CosmosDB:Database%", containerName: "%CosmosDB:Container%", Connection = "CosmosDB")]
  public dynamic?[] Run([QueueTrigger("%AuditTrail:QueueName%", Connection = "Storage")] QueueMessage message)
  {
    return [ new
    {
      id = message.MessageId,
      trail = message.Body.ToString(),
      createdAt = DateTime.UtcNow
    }];
  }
}

