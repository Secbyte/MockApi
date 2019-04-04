using System.Threading.Tasks;

#pragma warning disable CA1822 // Members that do not access instance data can be marked as static
namespace SecByte.MockApi.Server
{
    public interface IFileReader
    {
        Task<string> ReadContentsAsync(string file);
    }
}
