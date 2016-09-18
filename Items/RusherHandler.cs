using RusherLib.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RusherLib.Items {
    public class RusherHandler {
        public string handlerName;
        public AccessType accessType;

        public RusherHandler(string name, AccessType type) {
            handlerName = name;
            accessType = type;
        }
    }
}
