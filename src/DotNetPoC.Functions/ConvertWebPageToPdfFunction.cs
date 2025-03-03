using DotNetPoC.Functions.Models;
using DotNetPoC.Functions.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;


namespace DotNetPoC.Functions
{
  public class ConvertWebPageToPdfFunction(ILogger<ConvertWebPageToPdfFunction> logger,
   CloudStorage storage)
  {

    [Function(nameof(ConvertWebPageToPdfFunction))]
    public async Task<HttpResponseData> RunAsync(
      [HttpTrigger(AuthorizationLevel.Function, "post", Route = "page-to-pdf")] HttpRequestData req)
    {
      var requestAsJson = await StreamConverter.ToStringAsync(req.Body);
      var result = AppJsonSerializer.Deserialize<PageToPdfRequest>(requestAsJson);

      HttpResponseData response = req.CreateResponse();
      if (!result.IsSuccess)
      {
        if (result.Exception != null)
        {
          logger.LogError(result.Exception, "Exception occurred during json deserialization");
        }

        response.StatusCode = HttpStatusCode.BadRequest;
        response.WriteString("Input is not in valid format. Input must be json object" +
          " with url and generatedFileName property");
        return response;
      }

      if (string.IsNullOrWhiteSpace(result.Value!.Url))
      {
        response.StatusCode = HttpStatusCode.BadRequest;
        response.WriteString("url is required");
        return response;
      }

      logger.LogInformation("Converting {url} to pdf", result.Value.Url);

      using var pdfStream = await PdfGenerator.GenerateFromUrl(result.Value.Url);
      var pdfUrl = await storage.UploadWebPagePDfAsync(pdfStream, result.Value.GeneratedFileName);

      response.StatusCode = HttpStatusCode.OK;
      response.WriteString(pdfUrl);
      return response;
    }


  }
}
