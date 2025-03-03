using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Blobs;
using static System.Environment;
using DotNetPoC.Functions.Shared;
using SendGrid;

var builder = FunctionsApplication.CreateBuilder(args);
builder.ConfigureFunctionsWebApplication();

builder.Services.AddSingleton(() =>
  new BlobServiceClient(GetEnvironmentVariable("Storage"))
    .GetBlobContainerClient(GetEnvironmentVariable("PDFContainerName")));
builder.Services.AddSingleton<CloudStorage>();

builder.Services.AddSingleton(() =>
{
  var key = GetEnvironmentVariable("SENDGRID_KEY");
  return new SendGridClient(key);
});

builder.Build().Run();

