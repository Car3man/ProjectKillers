using ProtoBuf;
using System;
using System.Collections.Generic;

namespace ProjectKillersCommon {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class NetData {
        [ProtoMember(1)]
        public RequestTypes Type;
        [ProtoMember(2)]
        public Dictionary<string, ObjectWrapper> Values = new Dictionary<string, ObjectWrapper>();

        public NetData (RequestTypes type, Dictionary<string, ObjectWrapper> values) {
            Type = type;
            Values = values;
        }
    }
}
