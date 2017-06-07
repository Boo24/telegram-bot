using System;
using System.Diagnostics;
using source.Domain.Model;
using source.Infrastructure.Databases;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using source.App.Commands;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace source.App
{
    class TelegramHandler
    {
        private static readonly TelegramBotClient TelegramClient = new TelegramBotClient("364823821:AAHIBUfvkkykh-mRBsFTlPEGhOrAqpm1fkU");
        private IBot Bot { get; }

        public TelegramHandler(IBot bot)
        {
            Bot = bot;
          
            TelegramClient.OnCallbackQuery += BotOnCallbackQueryReceived;
            TelegramClient.OnMessage += BotOnMessageReceived;
            TelegramClient.OnMessageEdited += BotOnMessageReceived;
            TelegramClient.OnInlineQuery += BotOnInlineQueryReceived;
            TelegramClient.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            TelegramClient.OnReceiveError += BotOnReceiveError;

            var me = TelegramClient.GetMeAsync().Result;
        }
        public void Run()
        {
            TelegramClient.StartReceiving();
            Console.ReadLine();
            TelegramClient.StopReceiving();
        }
        public async void SendMessage(string message, long chatId)
        {
            await TelegramClient.SendTextMessageAsync(chatId, message);
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("BotOnReceiveError");
            Debugger.Break();
        }

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine("BotOnChosenInlineResultReceived");
            Console.WriteLine($"Received choosen inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            Console.WriteLine("BotOnMessageReceived");
            var message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.TextMessage) return;

            await TelegramClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            var answer = Bot.HandleCommand(message.Text);
            await TelegramClient.SendTextMessageAsync(message.Chat.Id, answer);
        }

        private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            Console.WriteLine("BotOnInlineQueryReceived");
            InlineQueryResult[] results = null;

            await TelegramClient.AnswerInlineQueryAsync(inlineQueryEventArgs.InlineQuery.Id, results, isPersonal: true, cacheTime: 0);
        }
        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            Console.WriteLine("BotOnCallbackQueryReceived");
            await TelegramClient.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                $"Received {callbackQueryEventArgs.CallbackQuery.Data}");
        }

        private static async void BotOnUpdate(EventHandler<UpdateEventArgs> args)
        {
            Console.WriteLine("OnUpdate");
            await TelegramClient.AnswerCallbackQueryAsync(null);

        }
    }
}
