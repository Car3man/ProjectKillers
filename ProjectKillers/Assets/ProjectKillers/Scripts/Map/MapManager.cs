using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : LocalSingletonBehaviour<MapManager> {
    [SerializeField] private LobbyHolder lobbyHolder;
    [SerializeField] private CreateRoomPanel createRoomPanel;
    [SerializeField] private RoomHolder roomHolder;

    private string eventID = string.Empty;

    private void Start() {
        lobbyHolder.gameObject.SetActive(true);
    }

    public void OnButtonRoomClicked(string id) {
        string enterRoomId = NetManager.I.Client.UnityEventReceiver.AddResponseObserver(OnEnteredInRoom, true);
        NetManager.I.Client.SendRequest(Utils.ToBytesJSON(new NetData(RequestTypes.EnterInRoom, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(id) } })), enterRoomId);
    }

    private void OnEnteredInRoom(byte[] data) {
        NetData netData = Utils.FromBytesJSON<NetData>(data);

        lobbyHolder.gameObject.SetActive(false);
        roomHolder.gameObject.SetActive(true);
        roomHolder.GetComponent<RoomHolder>().RoomID = netData.Values["id"].ObjectValue as string;

        string startMissionID = NetManager.I.Client.UnityEventReceiver.AddEventObserver(OnStartedMission, true);
    }

    public void OnButtonCreateRoomClicked() {
        createRoomPanel.gameObject.SetActive(true);
    }

    public void OnButtonLeaveRoomClicked() {

    }

    public void OnButtonStartMissionClicked(string roomID) {
        NetManager.I.Client.SendRequest(Utils.ToBytesJSON(new NetData(RequestTypes.StartMission, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(roomID) } })));
    }

    private void OnStartedMission(byte[] data) {
        NetDataEvent netData = Utils.FromBytesJSON<NetDataEvent>(data);

        TransitionManager.I.LoadGame();
    }
}
