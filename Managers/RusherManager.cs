using RusherLib.Items;
using System;
using System.Collections.Generic;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Speech.Recognition;
using System.Speech.AudioFormat;

namespace RusherLib.Managers {
    public class RusherManager {
        public TelegramBotClient TeleBot;
        public YandexBotClient YandexBot;
        public DatabaseManager Database;
        public object Handlers;

        public RusherManager(string teleKey, string yandexKey, object handlers) {
            Database = new DatabaseManager();
            this.Handlers = handlers;
            TeleBot = new TelegramBotClient(teleKey);
            YandexBot = new YandexBotClient(yandexKey);
            Database.LoadData();
        }
        public void Start() {
            TeleBot.OnMessage += TeleBot_OnMessage;
            TeleBot.StartReceiving();
        }
        public void Stop() {
            TeleBot.StopReceiving();
            Database.SaveData();
        }
        public void AddHandler(string word1, string word2, Action<RusherManager, RusherUser, Message, List<string>, List<string>> handler, AccessType access) {
            if (Database.handlers.ContainsKey(word1)) {
                if (!Database.handlers[word1].ContainsKey(word2)) {
                    Console.WriteLine(handler.GetMethodInfo().Name);
                    Database.handlers[word1].Add(word2, new RusherHandler(handler.GetMethodInfo().Name, access));
                } else {
                    Console.WriteLine("word2 exists");
                }
            } else {
                Database.handlers.Add(word1, new Dictionary<string, RusherHandler> { [word2] = new RusherHandler(handler.GetMethodInfo().Name, access) });
            }
        }
        private void TeleBot_OnMessage(object sender, MessageEventArgs e) {
            var userName = e.Message.From.Username;
            var chatId = e.Message.Chat.Id;
            if (userName == null)
                return;
            RusherUser user;
            if (Database.users.TryGetValue(userName, out user)) {
                switch (e.Message.Type) {
                    case MessageType.TextMessage:
                        string cmdType;
                        List<string> cmdBody, cmdArgs;
                        if (ParseManager.TryHandleTextRequest(e.Message.Text, out cmdType, out cmdBody, out cmdArgs)) {
                            if (user.messageType == MessageType.TextMessage) {
                                List<string> words;
                                if (ParseManager.TryGetWordsFromHandlers(Database.handlers, cmdType, out words)) {
                                    RusherHandler handler;
                                    if (ParseManager.TryGetHandlerFromHandlers(Database.handlers, words, out handler)) {
                                        if (user.accessType >= handler.accessType) {
                                            Handlers.GetType().GetMethod(handler.handlerName).Invoke(Handlers, new object[] { this, user, e.Message, cmdBody, cmdArgs });
                                        } else {
                                            TeleBot.SendTextMessageAsync(chatId, $"{user.userName}: Недостаточно прав");
                                        }
                                    } else {
                                        TeleBot.SendTextMessageAsync(chatId, $"{user.userName}: Отсутствует обработчик команды");
                                    }
                                } else {
                                    TeleBot.SendTextMessageAsync(chatId, $"{user.userName}: Некорректная команда");
                                }
                            } else {
                                TeleBot.SendTextMessageAsync(chatId, $"{user.userName}: Ожидается {user.messageType}");
                            }
                        }
                        break;
                    case MessageType.PhotoMessage:
                        Handlers.GetType().GetMethod("cmd_add_photo").Invoke(Handlers, new object[] { this, user, e.Message, null, null });
                        break;
                    case MessageType.AudioMessage:
                        Handlers.GetType().GetMethod("cmd_add_audio").Invoke(Handlers, new object[] { this, user, e.Message, null, null });
                        break;
                    case MessageType.VideoMessage:
                        Handlers.GetType().GetMethod("cmd_add_video").Invoke(Handlers, new object[] { this, user, e.Message, null, null });
                        break;
                    case MessageType.VoiceMessage:
                        Handlers.GetType().GetMethod("cmd_add_voice").Invoke(Handlers, new object[] { this, user, e.Message, null, null });
                        break;
                    case MessageType.DocumentMessage:
                        Handlers.GetType().GetMethod("cmd_add_document").Invoke(Handlers, new object[] { this, user, e.Message, null, null });
                        break;
                    case MessageType.StickerMessage:
                        Handlers.GetType().GetMethod("cmd_add_sticker").Invoke(Handlers, new object[] { this, user, e.Message, null, null });
                        break;
                }
            } else if (e.Message.Type == MessageType.TextMessage && e.Message.Text.StartsWith("Йолобот")) {
                TeleBot.SendTextMessageAsync(chatId, $"{e.Message.From.FirstName}: Вы должны быть авторизованы");
            }
        }
    }
}
