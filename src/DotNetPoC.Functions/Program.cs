using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Azure.Storage.Blobs;
using static System.Environment;
using DotNetPoC.Functions.Shared;
using SendGrid;
using Microsoft.Extensions.Configuration;


var builder = FunctionsApplication.CreateBuilder(args);
builder.ConfigureFunctionsWebApplication();
builder.Configuration.AddUserSecrets("c5d8bba1-c2ea-4e52-9a16-2ea72da91501");

builder.Services.AddSingleton<BlobContainerClient>(pr =>
  new BlobServiceClient(GetEnvironmentVariable("Storage"))
    .GetBlobContainerClient(GetEnvironmentVariable("PDFContainerName")));
builder.Services.AddSingleton<CloudStorage>();

builder.Services.AddSingleton<SendGridClient>(pr =>
{
  var key = GetEnvironmentVariable("SENDGRID_KEY");
  return new SendGridClient(key);
});

builder.Build().Run();

