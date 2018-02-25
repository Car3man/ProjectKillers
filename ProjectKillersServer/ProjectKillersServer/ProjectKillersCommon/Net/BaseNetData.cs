using ProtoBuf;
using System;
using System.Collections.Generic;

namespace ProjectKillersCommon {
    [Serializable]
    [ProtoContract(SkipConstructor = true, UseProtoMembersOnly = true)]
    [ProtoInclude(104, typeof(NetDataRequest))]
    [ProtoInclude(105, typeof(NetDataEvent))]
    public class BaseNetData {
        [ProtoMember(1)]
        public Dictionary<string, ObjectWrapper> Values = new Dictionary<string, ObjectWrapper>();
    }
}
