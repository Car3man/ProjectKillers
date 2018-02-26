using System.Collections.Generic;
using Common;
using ProjectKillersCommon;
using ProjectKillersServer.Controllers;
using SwiftKernelServerProject;

namespace ProjectKillersServer.Events {
    public static class NewWaveHandler {
        public static void DoHandle(RoomController room, int waveNumber) {
            Server.SendEvent(room.Clients, Utils.ToBytesJSON(new NetDataEvent(EventTypes.NewWave, new Dictionary<string, ObjectWrapper>() { { "wave_number", new ObjectWrapper<int>(waveNumber) } })), "EventWaveHandlerHandleNewWawe");
        }
    }
}
