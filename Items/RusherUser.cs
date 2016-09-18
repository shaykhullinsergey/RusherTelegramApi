using RusherLib.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RusherLib.Items {
    public class RusherUser {
        public string userName;
        public int userBalance;
        public string nextItemName;
        public AccessType accessType; 
        public MessageType messageType;

        public RusherUser() {

        }
        public RusherUser(string name, AccessType access) {
            userName = name;
            if (messageType == default(MessageType))
                messageType = MessageType.TextMessage;
            accessType = access;
        }
    }
}
