using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace RusherLib {
    public class YandexBotClient {
        private readonly string _yandexKey;
        private readonly WebClient _client = new WebClient();

        public YandexBotClient(string yandexKey) {
            _yandexKey = yandexKey;
        }
        public bool TryTranslate(string text, out string resp, string lang = "en") {
            string req = $"https://translate.yandex.net/api/v1.5/tr.json/translate?key={_yandexKey}&text={text}&lang={lang}";
            var js = JObject.Parse(Encoding.UTF8.GetString(_client.DownloadData(req)));
            JToken token;
            if (js.TryGetValue("text", out token)) {
                resp = token.ToString().TrimStart('[').TrimEnd(']').Trim();
                return true;
            } else {
                resp = "";
                return false;
            }
        }
    }
}
