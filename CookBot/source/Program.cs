using System;
using System.Linq;
using source.Domain.Model;
using source.Infrastructure.Databases;
using source.Infrastructure.Serialization;
using source.App.Commands;
using Ninject;
using source.App;
using System.Collections.Generic;

namespace source
{
    class Program
    {
        private const string DB = @"..\..\..\databases\ArrayDatabase.bin";

        static void Main(string[] args)
        {
            var container = new StandardKernel();

            container.Bind<IBot>().To<CookBot>();
            container.Bind<TelegramHandler>().ToSelf();

            container.Bind<IDatabase<Recipe>>().
                ToConstant(new ArrayDatabase<Recipe>(DB, new BinarySerializer()));

            container.Bind<IBotCommand>().To<RecipeByNameCommand>().InSingletonScope();
            container.Bind<IBotCommand>().To<RecipeByIngredientsCommand>().InSingletonScope();
            container.Bind<IBotCommand>().To<RecipeListCommand>().InSingletonScope();
            container.Bind<IBotCommand>().To<HelpCommand>();

            container.Bind<Lazy<List<IBotCommand>>>().ToConstant(new Lazy<List<IBotCommand>>(() => container
                                                                           .GetAll<IBotCommand>()
                                                                           .ToList()));

            var telegramHandler = container.Get<TelegramHandler>();
            telegramHandler.Run();
        }
        
    }
}
