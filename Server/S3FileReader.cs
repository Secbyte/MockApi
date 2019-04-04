using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.Extensions.Options;

#pragma warning disable CA1822 // Members that do not access instance data can be marked as static
namespace SecByte.MockApi.Server
{
    public class S3FileReader : IFileReader
    {
        private readonly string _bucketName;
        private readonly IAmazonS3 _s3Service;

        public S3FileReader(IAmazonS3 s3Service, IOptions<FileReaderOptions> fileReaderOptions)
        {
            _s3Service = s3Service;
            _bucketName = fileReaderOptions.Value.Root;
        }

        public async Task<string> ReadContentsAsync(string file)
        {
             var s3Response = await _s3Service.GetObjectAsync(_bucketName, file);
            if ((int)s3Response.HttpStatusCode != 200)
            {
                throw new FileLoadException($"Unable to retrieve the configuration document from {_bucketName}:{file}");
            }

            using (var streamReader = new StreamReader(s3Response.ResponseStream))
            {
                return await streamReader.ReadToEndAsync();
            }
        }
    }
}
