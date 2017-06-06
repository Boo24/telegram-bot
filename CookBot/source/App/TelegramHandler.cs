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

            //            Console.WriteLine(messageEventArgs.Message.Text);
            //            var message = messageEventArgs.Message;

            //            if (message == null || message.Type != MessageType.TextMessage) return;


            //            if (message.Text.StartsWith("/inline")) // send inline keyboard
            //            {
            //                await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            //                var keyboard = new InlineKeyboardMarkup(new[]
            //                {
            //                    new[] // first row
            //                    {
            //                        new InlineKeyboardButton("1.1"),
            //                        new InlineKeyboardButton("1.2"),
            //                    },
            //                    new[] // second row
            //                    {
            //                        new InlineKeyboardButton("2.1"),
            //                        new InlineKeyboardButton("2.2"),
            //                    }
            //                });

            //                await Task.Delay(500); // simulate longer running task

            //                await Bot.SendTextMessageAsync(message.Chat.Id, "Choose",
            //                    replyMarkup: keyboard);
            //            }
            //            else if (message.Text.StartsWith("/keyboard")) // send custom keyboard
            //            {
            //                var keyboard = new ReplyKeyboardMarkup(new[]
            //                {
            //                    new [] // first row
            //                    {
            //                        new KeyboardButton("1.1"),
            //                        new KeyboardButton("1.2"),
            //                    },
            //                    new [] // last row
            //                    {
            //                        new KeyboardButton("2.1"),
            //                        new KeyboardButton("2.2"),
            //                    }
            //                });

            //                await Bot.SendTextMessageAsync(message.Chat.Id, "Choose",
            //                    replyMarkup: keyboard);
            //            }
            //            //else if (message.Text.StartsWith("/photo")) // send a photo
            //            //{
            //            //    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

            //            //    const string file = @"<FilePath>";

            //            //    var fileName = file.Split('\\').Last();

            //            //    using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            //            //    {
            //            //        var fts = new FileToSend(fileName, fileStream);

            //            //        await Bot.SendPhotoAsync(message.Chat.Id, fts, "Nice Picture");
            //            //    }
            //            //}
            //            else if (message.Text.StartsWith("/request")) // request location or contact
            //            {
            //                var keyboard = new ReplyKeyboardMarkup(new[]
            //                {
            //                    new KeyboardButton("Location")
            //                    {
            //                        RequestLocation = true
            //                    },
            //                    new KeyboardButton("Contact")
            //                    {
            //                        RequestContact = true
            //                    },
            //                });

            //                await Bot.SendTextMessageAsync(message.Chat.Id, "Who or Where are you?", replyMarkup: keyboard);
            //            }
            //            else
            //            {
            //                var usage = @"Usage:
            //                /inline   - send inline keyboard
            //                /keyboard - send custom keyboard
            //                /photo    - send a photo
            //                /request  - request location or contact
            //";

            //                await Bot.SendTextMessageAsync(message.Chat.Id, usage,
            //                    replyMarkup: new ReplyKeyboardHide());
            //            }
        }

        private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            Console.WriteLine("BotOnInlineQueryReceived");
            InlineQueryResult[] results = {
                new InlineQueryResultLocation
                {
                    Id = "1",
                    Latitude = 40.7058316f, // displayed result
                    Longitude = -74.2581888f,
                    Title = "New York",
                    InputMessageContent = new InputLocationMessageContent // message if result is selected
                    {
                        Latitude = 40.7058316f,
                        Longitude = -74.2581888f,
                    }
                },

                new InlineQueryResultLocation
                {
                    Id = "2",
                    Longitude = 52.507629f, // displayed result
                    Latitude = 13.1449577f,
                    Title = "Berlin",
                    InputMessageContent = new InputLocationMessageContent // message if result is selected
                    {
                        Longitude = 52.507629f,
                        Latitude = 13.1449577f
                    }
                }
            };

            await TelegramClient.AnswerInlineQueryAsync(inlineQueryEventArgs.InlineQuery.Id, results, isPersonal: true, cacheTime: 0);
        }


        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            Console.WriteLine("BotOnCallbackQueryReceived");
            await TelegramClient.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                $"Received {callbackQueryEventArgs.CallbackQuery.Data}");
        }
    }
}
