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
    private string startMissionID = string.Empty;

    private void Start() {
        lobbyHolder.gameObject.SetActive(true);
    }

    //вход в комнату по кнопке
    public void OnButtonRoomClicked(string id) {
        string enterRoomId = NetManager.I.Client.UnityEventReceiver.AddResponseObserver(OnEnteredInRoom, true);
        NetManager.I.Client.SendRequest(Utils.ToBytesJSON(new NetDataRequest(RequestTypes.EnterInRoom, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(id) } })), enterRoomId);
    }

    //вошли в комнату
    private void OnEnteredInRoom(byte[] data) {
        NetDataRequest netData = Utils.FromBytesJSON<NetDataRequest>(data);

        lobbyHolder.gameObject.SetActive(false);
        roomHolder.GetComponent<RoomHolder>().RoomID = netData.Values["id"].ObjectValue as string;
        roomHolder.gameObject.SetActive(true);

        startMissionID = NetManager.I.Client.UnityEventReceiver.AddEventObserver(OnStartedMission, true);
    }

    //создаем комнату
    public void OnButtonCreateRoomClicked() {
        createRoomPanel.gameObject.SetActive(true);
    }

    //покидаем комнату
    public void OnButtonLeaveRoomClicked(string roomID) {
        string leaveRoomID = NetManager.I.Client.UnityEventReceiver.AddResponseObserver(OnLeavedRoom, true);
        NetManager.I.Client.SendRequest(Utils.ToBytesJSON(new NetDataRequest(RequestTypes.LeaveRoom, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(roomID) } })), leaveRoomID);
    }

    //покинули комнату
    private void OnLeavedRoom(byte[] data) {
        NetDataRequest netData = Utils.FromBytesJSON<NetDataRequest>(data);

        lobbyHolder.gameObject.SetActive(true);
        roomHolder.gameObject.SetActive(false);

        NetManager.I.Client.UnityEventReceiver.RemoveEventObserver(startMissionID);
    }

    //начинаем миссию
    public void OnButtonStartMissionClicked(string roomID) {
        NetManager.I.Client.SendRequest(Utils.ToBytesJSON(new NetDataRequest(RequestTypes.StartMission, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(roomID) } })));
    }

    //начали миссию 
    private void OnStartedMission(byte[] data) {
        NetDataEvent netData = Utils.FromBytesJSON<NetDataEvent>(data);

        TransitionManager.I.LoadGame();

        NetManager.I.Client.UnityEventReceiver.RemoveEventObserver(startMissionID);
    }
}
