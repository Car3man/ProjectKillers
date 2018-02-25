using ProtoBuf;
using System;
using System.Collections.Generic;

namespace ProjectKillersCommon {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class NetDataEvent : BaseNetData {
        [ProtoMember(1)]
        public EventTypes Type;

        public NetDataEvent(EventTypes type, Dictionary<string, ObjectWrapper> values) {
            Type = type;
            Values = values;
        }
    }
}
