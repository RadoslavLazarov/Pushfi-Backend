namespace Pushfi.Domain.Configuration
{
    public class AzureBlobStorageConfiguration
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public string BaseUrl { get; set; }
    }
}
