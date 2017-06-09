using System;
using System.Linq;
using source.Domain.Model;
using source.Infrastructure.Databases;
using source.Infrastructure.Serialization;
using source.App.Commands;
using Ninject;
using Ninject.Extensions.Conventions;
using source.App;
using System.Collections.Generic;
using System.IO;

namespace source
{
    class Program
    {
        static Stream databaseStream = new FileStream(@"..\..\..\databases\ArrayDatabase.bin", FileMode.Open);
        private const string HelloMessage = "Привет! Меня зовут Cook Bot! " +
                                            "У меня самые лучшие рецепты." +
                                            " Вот список команд, которые я могу выполнить: ";

        public static StandardKernel CreateKernel()
        {
            var container = new StandardKernel();
            container.Bind(x => x.FromThisAssembly()
                                .SelectAllClasses()
                                .InheritedFrom(typeof(IBotCommand))
                                .BindAllInterfaces()
                                .Configure(y => y.InSingletonScope()
                                ));
            container.Bind<string>().ToConstant(HelloMessage).Named("HelloMessage");
            container.Bind<IBot>().To<CookBot>();
            container.Bind<TelegramHandler>().ToSelf();

            container.Bind<IDatabase<Recipe>>().
                ToConstant(new ArrayDatabase<Recipe>(databaseStream, new BinarySerializer()));

            container.Bind<Lazy<List<IBotCommand>>>().
                ToConstant(new Lazy<List<IBotCommand>>(() => container.GetAll<IBotCommand>().ToList()));

            return container;
        }

        static void Main(string[] args)
        {
            var container = CreateKernel();
            var telegramHandler = container.Get<TelegramHandler>();
            telegramHandler.Run();
        }

    }
}
