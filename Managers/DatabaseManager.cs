using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using RusherLib.Items;
using  MsgPack.Serialization;

namespace RusherLib.Managers {
    public class DatabaseManager {
        public Dictionary<string, RusherUser> users;
        public Dictionary<string, Dictionary<string, RusherHandler>> handlers;

        public Dictionary<string, string> commands;
        public Dictionary<string, string> photos;
        public Dictionary<string, string> Audios;
        public Dictionary<string, string> Videos;
        public Dictionary<string, string> Documents;
        public Dictionary<string, string> Stickers;
        public Dictionary<string, string> Voices;
        public Dictionary<string, Lottery> lotteries;
        public Dictionary<string, List<string>> lists;
        public Dictionary<string, int> Prices;
        public Dictionary<string, string> Replacements;



        public void LoadData() {
            var msgStrRusher = SerializationContext.Default.GetSerializer<KeyValuePair<string, RusherUser>[]>();
            var msgStrStr = SerializationContext.Default.GetSerializer<KeyValuePair<string, string>[]>();
            var msgStrListstr = SerializationContext.Default.GetSerializer<KeyValuePair<string, List<string>>[]>();
            var msgStrLottery = SerializationContext.Default.GetSerializer<KeyValuePair<string, Lottery>[]>();
            var msgStrInt = SerializationContext.Default.GetSerializer<KeyValuePair<string, int>[]>();
            handlers = new Dictionary<string, Dictionary<string, RusherHandler>>();

            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\users.yolo", FileMode.OpenOrCreate, FileAccess.Read))
                users = stream.Length > 0 ? msgStrRusher.Unpack(stream).ToDictionary(x => x.Key, x => x.Value) : new Dictionary<string, RusherUser> { ["yolorusher"] = new RusherUser("yolorusher", AccessType.Admin) };
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\commands.yolo", FileMode.OpenOrCreate, FileAccess.Read))
                commands = stream.Length > 0 ? msgStrStr.Unpack(stream).ToDictionary(x => x.Key, x => x.Value) : new Dictionary<string, string>();
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\photos.yolo", FileMode.OpenOrCreate, FileAccess.Read))
                photos = stream.Length > 0 ? msgStrStr.Unpack(stream).ToDictionary(x => x.Key, x => x.Value) : new Dictionary<string, string>();
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\audios.yolo", FileMode.OpenOrCreate, FileAccess.Read))
                Audios = stream.Length > 0 ? msgStrStr.Unpack(stream).ToDictionary(x => x.Key, x => x.Value) : new Dictionary<string, string>();
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\videos.yolo", FileMode.OpenOrCreate, FileAccess.Read))
                Videos = stream.Length > 0 ? msgStrStr.Unpack(stream).ToDictionary(x => x.Key, x => x.Value) : new Dictionary<string, string>();
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\documents.yolo", FileMode.OpenOrCreate, FileAccess.Read))
                Documents = stream.Length > 0 ? msgStrStr.Unpack(stream).ToDictionary(x => x.Key, x => x.Value) : new Dictionary<string, string>();
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\stickers.yolo", FileMode.OpenOrCreate, FileAccess.Read))
                Stickers = stream.Length > 0 ? msgStrStr.Unpack(stream).ToDictionary(x => x.Key, x => x.Value) : new Dictionary<string, string>();
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\voices.yolo", FileMode.OpenOrCreate, FileAccess.Read))
                Voices = stream.Length > 0 ? msgStrStr.Unpack(stream).ToDictionary(x => x.Key, x => x.Value) : new Dictionary<string, string>();
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\prices.yolo", FileMode.OpenOrCreate, FileAccess.Read))
                Prices = stream.Length > 0 ? msgStrInt.Unpack(stream).ToDictionary(x => x.Key, x => x.Value) : new Dictionary<string, int>();
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\replacements.yolo", FileMode.OpenOrCreate, FileAccess.Read))
                Replacements = stream.Length > 0 ? msgStrStr.Unpack(stream).ToDictionary(x => x.Key, x => x.Value) : new Dictionary<string, string>();
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\lists.yolo", FileMode.OpenOrCreate, FileAccess.Read))
                lists = stream.Length > 0 ? msgStrListstr.Unpack(stream).ToDictionary(x => x.Key, x => x.Value) : new Dictionary<string, List<string>>();
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\lotterys.yolo", FileMode.OpenOrCreate, FileAccess.Read))
                lotteries = stream.Length > 0 ? msgStrLottery.Unpack(stream).ToDictionary(x => x.Key, x => x.Value) : new Dictionary<string, Lottery>();


            //prices.Add("Команда", 100);
            //prices.Add("Фото", 100);
            //prices.Add("Аудио", 100);
            //prices.Add("Видео", 100);
            //prices.Add("Документ", 100);
            //prices.Add("Стикер", 100);
            //prices.Add("Голос", 100);
        }

        public void SaveData() {
            var msgStrRusher = SerializationContext.Default.GetSerializer<KeyValuePair<string, RusherUser>[]>();
            var msgStrStr = SerializationContext.Default.GetSerializer<KeyValuePair<string, string>[]>();
            var msgStrListstr = SerializationContext.Default.GetSerializer<KeyValuePair<string, List<string>>[]>();
            var msgStrLottery = SerializationContext.Default.GetSerializer<KeyValuePair<string, Lottery>[]>();
            var msgStrInt = SerializationContext.Default.GetSerializer<KeyValuePair<string, int>[]>();

            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\users.yolo", FileMode.OpenOrCreate, FileAccess.Write))
                msgStrRusher.Pack(stream, users.Select(x => new KeyValuePair<string, RusherUser>(x.Key, x.Value)).ToArray());
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\commands.yolo", FileMode.OpenOrCreate, FileAccess.Write))
                msgStrStr.Pack(stream, commands.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToArray());
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\photos.yolo", FileMode.OpenOrCreate, FileAccess.Write))
                msgStrStr.Pack(stream, photos.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToArray());
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\audios.yolo", FileMode.OpenOrCreate, FileAccess.Write))
                msgStrStr.Pack(stream, Audios.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToArray());
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\videos.yolo", FileMode.OpenOrCreate, FileAccess.Write))
                msgStrStr.Pack(stream, Videos.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToArray());
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\documents.yolo", FileMode.OpenOrCreate, FileAccess.Write))
                msgStrStr.Pack(stream, Documents.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToArray());
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\stickers.yolo", FileMode.OpenOrCreate, FileAccess.Write))
                msgStrStr.Pack(stream, Stickers.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToArray());
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\voices.yolo", FileMode.OpenOrCreate, FileAccess.Write))
                msgStrStr.Pack(stream, Voices.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToArray());
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\prices.yolo", FileMode.OpenOrCreate, FileAccess.Write))
                msgStrInt.Pack(stream, Prices.Select(x => new KeyValuePair<string, int>(x.Key, x.Value)).ToArray());
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\replacements.yolo", FileMode.OpenOrCreate, FileAccess.Write))
                msgStrStr.Pack(stream, Replacements.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToArray());
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\lists.yolo", FileMode.OpenOrCreate, FileAccess.Write))
                msgStrListstr.Pack(stream, lists.Select(x => new KeyValuePair<string, List<string>>(x.Key, x.Value)).ToArray());
            using (var stream = new FileStream(Environment.CurrentDirectory + @"\data\lotterys.yolo", FileMode.OpenOrCreate, FileAccess.Write))
                msgStrLottery.Pack(stream, lotteries.Select(x => new KeyValuePair<string, Lottery>(x.Key, x.Value)).ToArray());
        }
    }

    public class Lottery {
        public string name;
        public Dictionary<string, Bet> bets;
        public int forWin;

        public Lottery() {

        }
        public Lottery(string name, int forWin) {
            this.name = name;
            bets = new Dictionary<string, Bet>();
            this.forWin = forWin;
        }

        public void AddBet(string name, int bet) {
            if (bets.ContainsKey(name)) {
                bets[name].bet += bet;
            } else {
                bets.Add(name, new Bet(name, bet));
            }
        }
        public string LaunchLottery() {
            var lastBet = 0;
            foreach (var bet in bets) {
                bet.Value.X1 = lastBet;
                bet.Value.X2 = lastBet + bet.Value.bet;
                lastBet += bet.Value.bet;
            }
            var rng = new Random().Next(0, lastBet);
            foreach (var bet in bets) {
                if (bet.Value.X1 <= rng && bet.Value.X2 > rng) {
                    return $"Победил {bet.Value.Name} с вероятностью {bet.Value.bet/lastBet * 100}%";
                }
            }
            return "Такого произойти не должно";
        }
    }
    public class Bet {
        public string Name;
        public int bet;
        public int X1, X2;

        public Bet() {

        }
        public Bet(string name, int bet) {
            Name = name;
            this.bet = bet;
        }
    }
}
