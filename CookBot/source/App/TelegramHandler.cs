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
            TelegramClient.OnMessage += BotOnMessageReceived;


            var me = TelegramClient.GetMeAsync().Result;
        }
        public void Run()
        {
            TelegramClient.StartReceiving();
        }
        public async void SendMessage(string message, long chatId)
        {
            await TelegramClient.SendTextMessageAsync(chatId, message);
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
    }
}
