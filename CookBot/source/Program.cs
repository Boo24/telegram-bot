using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using source.Domain.Model;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;
using source.Infrastructure.Databases;
using source.Infrastructure.Serialization;
using source.App.Commands;
using Ninject;
using source.App;
using source.Infrastructure;
using System.IO;

namespace source
{
    class Program
    {
        private static readonly TelegramBotClient Bot = new TelegramBotClient("364823821:AAHIBUfvkkykh-mRBsFTlPEGhOrAqpm1fkU");

        static void Main(string[] args)
        {
            var container = new StandardKernel();

            container.Bind<IBot>().To<CookBot>();
            container.Bind<TelegramHandler>().ToSelf();

            container.Bind<IDatabase<Recipe>>().
                ToConstant(new ArrayDatabase<Recipe>(@"..\..\..\databases\ArrayDatabase.bin", new BinarySerializer()));

            container.Bind<IBotCommand>().To<RecipeByNameCommand>();
            container.Bind<IBotCommand>().To<RecipeByIngredientsCommand>();
            container.Bind<IBotCommand>().To<RecipeListCommand>();

            // Тут циклическая ссылка + коллекция и все ломается
            //container.Bind<IBotCommand>().To<HelpCommand>(); 

            var bot = container.Get<IBot>();
            var telegramHandler = container.Get<TelegramHandler>();
            telegramHandler.Run();
        }
        
    }
}
