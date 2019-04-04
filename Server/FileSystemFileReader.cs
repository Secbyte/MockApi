using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

#pragma warning disable CA1822 // Members that do not access instance data can be marked as static
namespace SecByte.MockApi.Server
{
    public class FileSystemFileReader : IFileReader
    {
        private string _basePath;

        public FileSystemFileReader(IOptions<FileReaderOptions> fileReaderOptions)
        {
            _basePath = fileReaderOptions.Value.Root;
        }

        public Task<string> ReadContentsAsync(string file)
        {
            return System.IO.File.ReadAllTextAsync(Path.Combine(_basePath, file));
        }
    }
}
