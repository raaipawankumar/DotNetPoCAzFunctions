using Azure.Storage.Blobs;
using DotNetPoC.Functions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PuppeteerSharp;

namespace DotNetPoC.Functions
{
  public class ConvertWebPageToPdf(ILogger<ConvertWebPageToPdf> logger, IConfiguration configuration)
  {
    private const string FunctionName = "ConvertWebPageToPdf";
    private readonly ILogger<ConvertWebPageToPdf> _logger = logger;

    [Function(FunctionName)]
    public async Task Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
      _logger.LogInformation($"{FunctionName} started.");

      using var streamReader = new StreamReader(req.Body);
      var body = await streamReader.ReadToEndAsync();

      var pageRequest = JsonConvert.DeserializeObject<WebPageRequest>(body)
        ?? throw new InvalidOperationException($"{body} has invalid data.");

      if (string.IsNullOrWhiteSpace(pageRequest.Url))
      {
        _logger.LogError("url is not valid");
        throw new ArgumentException("Url is null or empty");
      }

      _logger.LogInformation($"Processing url: {pageRequest.Url}");

      using var pdfStream = await GetPdfFromUrl(pageRequest.Url);
      var blobConnectionString = configuration.GetConnectionString("BlobConnection");
      var containerName = configuration.GetValue<string>("ContainerName");
      var blobClient = new BlobClient(blobConnectionString, containerName, pageRequest.GeneratedFileName);
      await blobClient.UploadAsync(pdfStream);
    }
    private async Task<Stream> GetPdfFromUrl(string url)
    {
      var browserFetcher = new BrowserFetcher();
      await browserFetcher.DownloadAsync();
      await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
      await using var browserPage = await browser.NewPageAsync();
      await browserPage.GoToAsync(url);
      await browserPage.EvaluateExpressionHandleAsync("document.fonts.ready");
      return await browserPage.PdfStreamAsync();
    }
  }
}
