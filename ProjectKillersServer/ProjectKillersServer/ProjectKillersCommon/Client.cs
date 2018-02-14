﻿using ProjectKillersCommon.Classes;
using ProtoBuf;
using SwiftKernelCommon.Core;
using System;

namespace ProjectKillersCommon {
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    public class Client {
        public NetPeer Peer;

        [ProtoMember(1)]
        public Vector3K Position;
        [ProtoMember(2)]
        public Vector3K EulerAngles;
        [ProtoMember(3)]
        public string ID;
        [ProtoMember(4)]
        public bool Actualy = false;

        public Client(NetPeer peer, Vector3K position, Vector3K eulerAngles, string id) {
            Peer = peer;
            Position = position;
            EulerAngles = eulerAngles;
            ID = id;
        }

        public Client(NetPeer peer) {
            Peer = peer;
        }
    }
}
