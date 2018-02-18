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
        NetManager.I.Client.SendRequest(Utils.ToBytesJSON(new NetData(RequestTypes.SyncRoom, new Dictionary<string, ObjectWrapper>())), requestID);
    }

    private void OnSyncRoom(byte[] data) {
        for (int i = 0; i < playerButtonContent.childCount; i++) {
            Destroy(playerButtonContent.GetChild(i).gameObject);
        }

        NetData ndata = Utils.FromBytesJSON<NetData>(data);
        List<Room> rooms = ndata.Values["rooms"].ObjectValue as List<Room>;

        foreach (Room r in rooms) {
            GameObject mobj = Instantiate(playerButtonPrefab, playerButtonContent);
            mobj.GetComponent<RoomButton>().Init(r.Name);
        }
    }

    private void OnDisable() {
        NetManager.I.Client.UnityEventReceiver.RemoveEventObserver(eventID);
    }

    public void OnButtonLeaveRoomClicked() {
        MapManager.I.OnButtonLeaveRoomClicked();
    }

    public void OnButtonRemoveRoomClicked() {

    }

    public void OnButtonStartMissionClicked() {
        MapManager.I.OnButtonStartMissionClicked(RoomID);
    }
}
