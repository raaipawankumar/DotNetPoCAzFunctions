using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;
using DotNetPoC.Functions.Models;

namespace DotNetPoC.Functions;

public class AddAuditTrailToDbFunction()
{
  [Function(nameof(AddAuditTrailToDbFunction))]
  public CosmosDBResponse? Run(
    [QueueTrigger("%AuditTrail:QueueName%", Connection = "Storage")] QueueMessage message)
  {

    var auditDocument = JsonConvert.SerializeObject(new
    {
      Id = message.MessageId,
      Trail = message.Body.ToString(),
      CreatedAt = DateTime.UtcNow
    },
    AppJsonSerializer.Default);

    CosmosDBResponse response = new()
    {
      Document = auditDocument
    };

    return response;
  }
}

