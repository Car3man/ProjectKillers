using ProtoBuf;
using System;

namespace ProjectKillersCommon {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class Client {
        [ProtoMember(1)]
        public string ID;
        [ProtoMember(2)]
        public string Nickname = "";

        public Client(string id) {
            ID = id;
            Nickname = "";
        }

        public Client() {
            ID = Guid.NewGuid().ToString();
            Nickname = "";
        }
    }
}
