using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DotNetPoC.Functions.Shared
{
    public static class PdfGenerator
    {
      public static async Task<Stream> GenerateFromUrl(string url)
    {
      var browserFetcher = new BrowserFetcher();
      await browserFetcher.DownloadAsync(BrowserTag.Dev);
      await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
      await using var browserPage = await browser.NewPageAsync();
      await browserPage.GoToAsync(url);
      await browserPage.EvaluateExpressionHandleAsync("document.fonts.ready");
      return await browserPage.PdfStreamAsync();
    }
  }
}
