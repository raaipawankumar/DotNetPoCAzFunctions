using Azure.Storage.Queues.Models;
using DotNetPoC.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.CosmosDB;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DotNetPoC.Functions;

public class AddAuditTrailToDbFunction(ILogger<AddAuditTrailToDbFunction> logger)
{

  [Function(nameof(AddAuditTrailToDbFunction))]
  public CosmosDBResponse Run(
    [QueueTrigger("%AuditTrail:QueueName%", Connection = "Storage")] QueueMessage message)
  {
    logger.LogInformation("{FunctionName} started at {DateTime}", nameof(AddAuditTrailToDbFunction), DateTime.UtcNow);
    var document = JsonConvert.SerializeObject(new
    {
      Id = message.MessageId,
      Trail = message.Body.ToString(),
      CreatedAt = DateTime.UtcNow
    }, new JsonSerializerSettings { Formatting = Formatting.Indented, ContractResolver = new CamelCasePropertyNamesContractResolver() });
    logger.LogInformation(document);

    var response = new CosmosDBResponse();
    response.Document = document;
    logger.LogInformation("{FunctionName} ended at {DateTime}", nameof(AddAuditTrailToDbFunction), DateTime.UtcNow);
    return response;

  }
}

