using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Missions;
using ProjectKillersCommon.Data.Objects;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectKillersCommon {
    [ProtoContract]
    [ProtoInclude(1, typeof(ObjectWrapper<int>))]
    [ProtoInclude(2, typeof(ObjectWrapper<decimal>))]
    [ProtoInclude(3, typeof(ObjectWrapper<DateTime>))]
    [ProtoInclude(4, typeof(ObjectWrapper<string>))]
    [ProtoInclude(5, typeof(ObjectWrapper<float>))]
    [ProtoInclude(6, typeof(ObjectWrapper<Vector3K>))]
    [ProtoInclude(7, typeof(ObjectWrapper<BaseMission>))]
    [ProtoInclude(8, typeof(ObjectWrapper<BaseMissionObject>))]
    [ProtoInclude(9, typeof(ObjectWrapper<List<Client>>))]
    [ProtoInclude(10, typeof(ObjectWrapper<bool>))]
    public abstract class ObjectWrapper {
        protected ObjectWrapper() { }
        abstract public object ObjectValue { get; set; }
    }

    [ProtoContract()]
    public class ObjectWrapper<T> : ObjectWrapper {
        public ObjectWrapper() : base() { }
        public ObjectWrapper(T t) { this.Value = t; }

        [ProtoIgnore()]
        public override object ObjectValue {
            get { return Value; }
            set { Value = (T)value; }
        }

        [ProtoMember(1)]
        public T Value { get; set; }
    }
}
