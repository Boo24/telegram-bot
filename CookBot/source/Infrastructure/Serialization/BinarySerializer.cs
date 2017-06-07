using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace source.Infrastructure.Serialization
{
    public class BinarySerializer : ISerializer
    {
        private IFormatter formatter = new BinaryFormatter();

        public void Serialize<T>(T obj, Stream stream) =>
            formatter.Serialize(stream, obj);

        public T Deserialize<T>(Stream stream) => 
            (T)formatter.Deserialize(stream);
    }
}
