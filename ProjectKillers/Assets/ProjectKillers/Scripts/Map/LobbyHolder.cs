using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyHolder : MonoBehaviour {
    [SerializeField] private Transform roomButtonContent;
    [SerializeField] private GameObject roomButtonPrefab;

    private string eventID = string.Empty;

    private void OnEnable() {
        eventID = NetManager.I.Client.UnityEventReceiver.AddEventObserver(OnGetRooms, false);

        string requestID = NetManager.I.Client.UnityEventReceiver.AddResponseObserver(OnGetRooms, true);
        NetManager.I.Client.SendRequest(Utils.ToBytesJSON(new NetDataRequest(RequestTypes.GetRooms, new Dictionary<string, ObjectWrapper>())), requestID);
    }

    private void OnGetRooms(byte[] data) {
        for (int i = 0; i < roomButtonContent.childCount; i++) {
            Destroy(roomButtonContent.GetChild(i).gameObject);
        }

        BaseNetData ndata = Utils.FromBytesJSON<BaseNetData>(data);
        List<Room> rooms = ndata.Values["rooms"].ObjectValue as List<Room>;

        foreach (Room r in rooms) {
            GameObject mobj = Instantiate(roomButtonPrefab, roomButtonContent);
            mobj.GetComponent<RoomButton>().Init(r.Name);
            mobj.GetComponent<RoomButton>().ID = r.ID;
        }
    }

    private void OnDisable() {
        NetManager.I.Client.UnityEventReceiver.RemoveEventObserver(eventID);
    }
}
