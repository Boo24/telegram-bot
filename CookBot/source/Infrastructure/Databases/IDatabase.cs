using System;
using System.Collections.Generic;

namespace CookBot.Infrastructure.Databases
{
    public interface IDatabase<T>
    {
        IEnumerable<T> GetAllSuitable(Func<T, bool> condition);
        T GetAnySuitable(Func<T, bool> condition);
    }
}
