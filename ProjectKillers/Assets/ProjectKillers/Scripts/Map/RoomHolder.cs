using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHolder : MonoBehaviour {
    [SerializeField] private Transform playerButtonContent;
    [SerializeField] private GameObject playerButtonPrefab;

    public string RoomID = string.Empty;

    private string eventID = string.Empty;

    private void OnEnable() {
        eventID = NetManager.I.Client.UnityEventReceiver.AddEventObserver(OnSyncRoom, false);

        string requestID = NetManager.I.Client.UnityEventReceiver.AddResponseObserver(OnSyncRoom, true);
        NetManager.I.Client.SendRequest(Utils.ToBytesJSON(new NetDataRequest(RequestTypes.SyncRoom, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(RoomID) } })), requestID);
    }

    private void OnSyncRoom(byte[] data) {
        for (int i = 0; i < playerButtonContent.childCount; i++) {
            Destroy(playerButtonContent.GetChild(i).gameObject);
        }

        BaseNetData ndata = Utils.FromBytesJSON<BaseNetData>(data);
        Room room = ndata.Values["room"].ObjectValue as Room;

        foreach (Client c in room.Clients) {
            GameObject mobj = Instantiate(playerButtonPrefab, playerButtonContent);
            mobj.GetComponent<PlayerButton>().Init(c.Nickname);
        }
    }

    private void OnDisable() {
        NetManager.I.Client.UnityEventReceiver.RemoveEventObserver(eventID);
    }

    public void OnButtonLeaveRoomClicked() {
        MapManager.I.OnButtonLeaveRoomClicked(RoomID);
    }

    public void OnButtonRemoveRoomClicked() {

    }

    public void OnRoomNameInputChanged(string value) {

    }

    public void OnMissionTypeDropdownChanged(int value) {

    }

    public void OnButtonStartMissionClicked() {
        MapManager.I.OnButtonStartMissionClicked(RoomID);
    }
}
