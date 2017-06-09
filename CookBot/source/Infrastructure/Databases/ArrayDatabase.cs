using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using source.Infrastructure.Serialization;

namespace source.Infrastructure.Databases
{
    public class ArrayDatabase<T> : IDatabase<T>
    {
        private T[] Data { get; }
        private ISerializer Serializer { get; }

        public ArrayDatabase(Stream stream, ISerializer serializer)
        {
            Serializer = serializer;
            Data = LoadDatabase(stream);
        }

        public IEnumerable<T> GetAllSuitable(Func<T, bool> condition) =>
            Data.Where(condition);

        public T GetAnySuitable(Func<T, bool> condition)
        {
            try
            {
                return Data.First(condition);
            }
            catch (InvalidOperationException a)
            {
                throw new InvalidOperationException("Нет подходящего элемента в базе данных",a);
            }
        }

        private T[] LoadDatabase(Stream stream)
        {
            using (stream)
            {
                return Serializer.Deserialize<T[]>(stream);
            }
        }
    }
}
