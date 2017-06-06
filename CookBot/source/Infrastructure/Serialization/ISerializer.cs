using System.IO;

namespace source.Infrastructure.Serialization
{
    public interface ISerializer
    {
        void Serialize<T>(T obj, Stream stream);
        T Deserialize<T>(Stream stream);
    }
}
