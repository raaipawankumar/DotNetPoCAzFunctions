using Azure.Storage.Blobs;
using DotNetPoC.Functions.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PuppeteerSharp;
using static System.Environment;

namespace DotNetPoC.Functions
{
  public class ConvertWebPageToPdfFunction(ILogger<ConvertWebPageToPdfFunction> logger)
  {
    private const string FunctionName = "ConvertWebPageToPdf";


    [Function(FunctionName)]
    public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {

      var webPageRequest = await GetWebPageRequestFromRequestAsync(req);
      if (webPageRequest == null)
      {
        return new BadRequestObjectResult("Input is not in valid format");
      }

      if (string.IsNullOrWhiteSpace(webPageRequest.Url))
      {
        return new BadRequestObjectResult("url is empty");
      }

      logger.LogInformation("Converting {url} to pdf", webPageRequest.Url);

      using var pdfStream = await GetPdfFromUrl(webPageRequest.Url);
      await UploadPdfAsync(webPageRequest.GeneratedFileName, pdfStream);

      return new OkObjectResult("Web page is converted");
    }
    private async static Task<WebPageRequest?> GetWebPageRequestFromRequestAsync(HttpRequest req)
    {
      using var streamReader = new StreamReader(req.Body);
      var body = await streamReader.ReadToEndAsync();
      return JsonConvert.DeserializeObject<WebPageRequest>(body);
    }
    private static async Task<Stream> GetPdfFromUrl(string url)
    {
      var browserFetcher = new BrowserFetcher();
      await browserFetcher.DownloadAsync(BrowserTag.Dev);
      await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
      await using var browserPage = await browser.NewPageAsync();
      await browserPage.GoToAsync(url);
      await browserPage.EvaluateExpressionHandleAsync("document.fonts.ready");
      return await browserPage.PdfStreamAsync();
    }
    private static async Task UploadPdfAsync(string fileName, Stream pdfStream)
    {
      var blobConnectionString = GetEnvironmentVariable("Storage");
      var containerName = GetEnvironmentVariable("PDFContainerName");
      var newFileName = $"{fileName}-{DateTime.UtcNow.ToTimestamp()}";
      var blobClient = new BlobClient(blobConnectionString, containerName, newFileName);
      await blobClient.UploadAsync(pdfStream);
    }
  }
}
