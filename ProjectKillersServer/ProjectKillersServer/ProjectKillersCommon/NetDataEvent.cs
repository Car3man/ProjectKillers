using ProtoBuf;
using System;
using System.Collections.Generic;

namespace ProjectKillersCommon {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class NetDataEvent {
        [ProtoMember(1)]
        public EventTypes Type;
        [ProtoMember(2)]
        public Dictionary<string, ObjectWrapper> Values = new Dictionary<string, ObjectWrapper>();

        public NetDataEvent(EventTypes type, Dictionary<string, ObjectWrapper> values) {
            Type = type;
            Values = values;
        }
    }
}
