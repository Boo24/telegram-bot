using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CookBot.Infrastructure.Serialization;

namespace CookBot.Infrastructure.Databases
{
    public class ArrayDatabase<T> : IDatabase<T>
    {
        public T[] Data { get; set; }
        public ISerializer Serializer { get; }

        public ArrayDatabase(string fileName, ISerializer serializer)
        {
            Serializer = serializer;
            Data = LoadDatabase(fileName);
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

        private T[] LoadDatabase(string fileName)
        {
            using (Stream stream = File.OpenRead(fileName))
            {
                return Serializer.Deserialize<T[]>(stream);
            }
        }

        public void SaveDatabase(string fileName)
        {
            using (Stream stream = File.OpenWrite(fileName))
            {
                Serializer.Serialize(Data, stream);
            }
        }
    }
}
