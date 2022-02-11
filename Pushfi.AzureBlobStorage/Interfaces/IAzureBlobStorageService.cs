
using Microsoft.AspNetCore.Http;
using Pushfi.Domain.Entities;

namespace Pushfi.AzureBlobStorage.Interfaces
{
    public interface IAzureBlobStorageService
    {
        Task<string> UploadFileAsync(string fileName, MemoryStream fileData, string fileMimeType);

        Task<EntityImage> UploadOriginalImage(IFormFile originalImage, string entityName, string propertyName);

        Task<EntityFile> UploadAttachmentAsync(IFormFile attachment, string entityName, string propertyName);
    }
}
