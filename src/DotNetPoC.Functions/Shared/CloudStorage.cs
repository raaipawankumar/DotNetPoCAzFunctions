using static System.Environment;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;


namespace DotNetPoC.Functions.Shared;

 public class CloudStorage(BlobContainerClient blobcontainerClient)
 {
  public async Task<string> UploadWebPagePDfAsync(Stream stream, string fileName)
  {
    var blobClient = blobcontainerClient.GetBlobClient(fileName);
   
    await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = "application/pdf" });
    return blobClient.Uri.AbsoluteUri;
  }

}
