using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Pushfi.AzureBlobStorage.Interfaces;
using Pushfi.Domain.Configuration;
using Pushfi.Domain.Entities;
using Pushfi.Domain.Exceptions;
using Pushfi.Domain.Resources;
using System.Drawing;

namespace Pushfi.AzureBlobStorage.Services
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly AzureBlobStorageConfiguration _azureBlobStorageConfiguration;

        public AzureBlobStorageService(IOptionsMonitor<AzureBlobStorageConfiguration> optionsMonitor)
        {
            this._azureBlobStorageConfiguration = optionsMonitor.CurrentValue;
        }

        //public async Task<string> UploadFileAsync2(string fileName, byte[] fileData, string fileMimeType)
        //{
        //    _ = fileData ?? throw new ArgumentNullException(nameof(fileData));
        //    _ = fileName ?? throw new ArgumentNullException(nameof(fileName));

        //    var cloudStorageAccount = CloudStorageAccount.Parse(_azureBlobStorageConfiguration.ConnectionString);
        //    var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        //    var cloudBlobContainer = cloudBlobClient.GetContainerReference(_azureBlobStorageConfiguration.ContainerName);
        //    if (await cloudBlobContainer.CreateIfNotExistsAsync())
        //    {
        //        await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
        //    }

        //    var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
        //    cloudBlockBlob.Properties.ContentType = fileMimeType;
        //    await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
        //    return cloudBlockBlob.Uri.AbsoluteUri;
        //}

        public async Task<string> UploadFileAsync(string fileName, MemoryStream fileData, string fileMimeType)
		{
			_ = fileData ?? throw new ArgumentNullException(nameof(fileData));
			_ = fileName ?? throw new ArgumentNullException(nameof(fileName));

			fileData.Position = 0;

			//var client = new BlobClient(
			//	this._azureBlobStorageConfiguration.ConnectionString,
			//	this._azureBlobStorageConfiguration.ContainerName,
			//	fileName
			//);

			var blobServiceClient = new BlobServiceClient(this._azureBlobStorageConfiguration.ConnectionString);
			var containerClient = blobServiceClient.GetBlobContainerClient(this._azureBlobStorageConfiguration.ContainerName);
			var blob = containerClient.GetBlobClient(fileName);

            var blobHttpHeader = new BlobHttpHeaders { ContentType = fileMimeType };

			var uploadedBlob = await blob.UploadAsync(fileData, new BlobUploadOptions { HttpHeaders = blobHttpHeader });
			//uploadedBlob.
			//await client.UploadAsync(fileData);

			//return client.Uri.AbsoluteUri;
			return blob.Uri.AbsoluteUri;
		}

		public async Task<EntityImage> UploadOriginalImage(IFormFile originalImage, string entityName, string propertyName)
		{
			if (originalImage == null)
			{
				throw new ArgumentNullException(nameof(originalImage));
			}

			if (string.IsNullOrWhiteSpace(entityName))
			{
				throw new ArgumentNullException(nameof(entityName));
			}

			if (string.IsNullOrWhiteSpace(propertyName))
			{
				throw new ArgumentNullException(nameof(propertyName));
			}

			EntityImage result = null;

			using (var ms = new MemoryStream())
			{
				var imageId = Guid.NewGuid();
				await originalImage.CopyToAsync(ms);
				var originalImageFileBytes = ms.ToArray();
				var originalImageFileName = $"{entityName}/{propertyName}/{imageId}{Path.GetExtension(originalImage.FileName)}";
				var originalImageFilePath = await UploadFileAsync(originalImageFileName, ms, originalImage.ContentType);
				var img = Image.FromStream(ms);		

				result = new EntityImage()
				{
					Url = originalImageFilePath,
					OriginalFileName = originalImage.FileName,
					MimeType = originalImage.ContentType.ToLower(),
					Width = img.Width,
					Height = img.Height,
				};
			}

			return result;
		}

		public async Task<EntityFile> UploadAttachmentAsync(IFormFile attachment, string entityName, string propertyName)
		{
			if (attachment == null)
			{
				throw new ArgumentNullException(nameof(attachment));
			}

			if (string.IsNullOrWhiteSpace(entityName))
			{
				throw new ArgumentNullException(nameof(entityName));
			}

			if (string.IsNullOrWhiteSpace(propertyName))
			{
				throw new ArgumentNullException(nameof(propertyName));
			}

			EntityFile result = null;

			using (var ms = new MemoryStream())
			{
				var imageId = Guid.NewGuid();
				await attachment.CopyToAsync(ms);
				var originalImageFileBytes = ms.ToArray();
				var originalImageFileName = $"{entityName}/{propertyName}/{imageId}{Path.GetExtension(attachment.FileName)}";
				var originalImageFilePath = await UploadFileAsync(originalImageFileName, ms, attachment.ContentType);

				result = new EntityFile()
				{
					Url = originalImageFilePath,
					OriginalFileName = attachment.FileName,
					MimeType = attachment.ContentType.ToLower(),
				};
			}

			return result;
		}
	}
}
