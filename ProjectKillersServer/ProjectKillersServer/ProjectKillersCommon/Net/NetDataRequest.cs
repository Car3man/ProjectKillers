using ProtoBuf;
using System;
using System.Collections.Generic;

namespace ProjectKillersCommon {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class NetDataRequest : BaseNetData {
        [ProtoMember(1)]
        public RequestTypes Type;
        [ProtoMember(2)]
        public RequestResult Result;

        public NetDataRequest (RequestTypes type, Dictionary<string, ObjectWrapper> values) {
            Type = type;
            Values = values;

            Result = RequestResult.Ok;
        }

        public NetDataRequest(RequestTypes type, RequestResult result, Dictionary<string, ObjectWrapper> values) {
            Type = type;
            Values = values;
            Result = result;
        }
    }
}
