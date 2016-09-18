using RusherLib.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RusherLib.Managers {
    public class ParseManager {
        public static bool TryGetAllWithAccess(Dictionary<string, RusherUser> users, AccessType access, out Dictionary<string, RusherUser> accessUser) {
            accessUser = new Dictionary<string, RusherUser>();
            foreach (var user in users) {
                if(user.Value.accessType == access) {
                    accessUser.Add(user.Key, user.Value);
                }
            }
            if (accessUser.Count > 0)
                return true;
            return false;
        }
        /// <summary>
        /// Ищет все команды из dictionary совпадающие с command
        /// </summary>
        public static bool TryGetWordsFromHandlers<TValue> (Dictionary<string, Dictionary<string, TValue>> dictionary, string command, out List<string> words) {
            words = new List<string>();
            foreach (var cmd1 in dictionary.Keys) {
                if (command.StartsWith(cmd1)) {
                    command = command.Remove(0, cmd1.Length).Trim();
                    words.Add(cmd1);
                    foreach (var cmd2 in dictionary[words[0]].Keys) {
                        if (cmd2.Trim().Equals(command.Trim())) {
                            command = command.Remove(0, cmd2.Length).Trim();
                            words.Add(cmd2);
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }

        public static bool TryGetHandlerFromHandlers<TValue>(Dictionary<string, Dictionary<string, TValue>> dictionary, List<string> words, out TValue handler) {
            Dictionary<string, TValue> dict1;
            if(dictionary.TryGetValue(words[0], out dict1)) {
                if(dict1.TryGetValue(words[1], out handler)) {
                    return true;
                }
            }
            handler = default(TValue);
            return false;
        }

        /// <summary>
        /// Если в dictionary еще нет псевдонима, состоящего из words, то он его добавит
        /// </summary>
        public static string TryAddPseudoToHandlers<TValue>(Dictionary<string, Dictionary<string, TValue>> dictionary, List<string> words, string pseudo) {
            Dictionary<string, TValue> word1;
            if (dictionary.TryGetValue(words[0], out word1)) {
                TValue handler;
                if (word1.TryGetValue(words[1], out handler)) {
                    if (!word1.ContainsKey(pseudo)) {
                        word1.Add(pseudo, handler);
                        return "Псевдоним добавлен1";
                    } else {
                        return "Псевдоним уже существует2";
                    }
                } else {
                    return "Комбинации не существует3";
                }
            } else {
                return "Такой команды не существует4";
            }
        }

        /// <summary>
        /// Если команда начинается с Йолобот, то разбивает на лексемы () и аргументы после () через пробел
        /// </summary>
        public static bool TryHandleTextRequest(string msg, out string cmdType, out List<string> cmdBody, out List<string> cmdArgs) {
            cmdType = "";
            cmdBody = new List<string>();
            cmdArgs = new List<string>();
            if (!msg.StartsWith("Йолобот"))
                return false;
            msg = msg.Remove(0, "Йолобот".Length).Trim();
            if (msg.IndexOf("[") > 0) {
                cmdType = msg.Substring(0, msg.IndexOf("[")).Trim();
                msg = msg.Replace(cmdType, "");
            } else {
                cmdType = msg;
                return true;
            }
            while (msg.Contains("[") && msg.Contains("]")) {
                int start = msg.IndexOf("[");
                int length = msg.IndexOf("]") - start + 1;
                cmdBody.Add(msg.Substring(start, length).Trim().TrimStart('[').TrimEnd(']'));
                msg = msg.Remove(start, length);
            }
            if (!string.IsNullOrWhiteSpace(msg))
                cmdArgs = msg.Trim().Split(' ').ToList();
            return true;
        }
    }
}
