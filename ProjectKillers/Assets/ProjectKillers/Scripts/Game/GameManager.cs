using Common;
using ProjectKillersCommon;
using ProjectKillersCommon.Classes;
using ProjectKillersCommon.Data.Missions;
using ProjectKillersCommon.Data.Objects;
using ProjectKillersCommon.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : LocalSingletonBehaviour<GameManager> {
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private int sendRateMission = 10;

    private List<Player> players = new List<Player>();
    private string syncPlayerNetworkID = "";
    private string shootPlayerNetworkID = "";
    private string syncMissionNetworkID = "";
	private string leaveMissionNetworkID = "";

    public string SyncPlayerNetworkID {
        get {
            return syncPlayerNetworkID;
        }
    }

    public string ShootPlayerNetworkID {
        get {
            return shootPlayerNetworkID;
        }
    }

    public string SyncMissionNetworkID {
        get {
            return syncMissionNetworkID;
        }
    }

	public string LeaveMissionNetworkID {
		get {
			return leaveMissionNetworkID;
		}
	}

    public override void DoAwake() {
        string requestID = NetManager.I.Client.UnityEventReceiver.AddResponseObserver(OnEnterMission, false);
        NetManager.I.Client.SendRequest(Utils.ToBytesJSON(new NetData(RequestTypes.EnterInMission, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(NetManager.I.ID) } })), requestID);
    }

    private IEnumerator MissionUpdater() {
        while (true) {
            for (int i = 0; i < 60 / sendRateMission; i++)
                yield return new WaitForEndOfFrame();
            if (!string.IsNullOrEmpty(SyncMissionNetworkID))
                NetManager.I.Client.SendRequest(Utils.ToBytesJSON(new NetData(RequestTypes.SyncMission, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(NetManager.I.ID) } })), SyncMissionNetworkID);
        }
    }

    private void OnEnterMission(byte[] data) {
        NetData ndata = Utils.FromBytesJSON<NetData>(data);

        if (ndata.Values.ContainsKey("clients")) {
            List<Client> clients = (List<Client>)ndata.Values["clients"].ObjectValue ?? new List<Client>();

            Debug.Log(clients);

            foreach (Client c in clients) {
                SpawnPlayer(c.ID, Vector3.zero);
            }

            syncPlayerNetworkID = NetManager.I.Client.UnityEventReceiver.AddResponseObserver(HandleSyncPlayers, false);
            shootPlayerNetworkID = NetManager.I.Client.UnityEventReceiver.AddResponseObserver(HandleShootPlayer, false);
            syncMissionNetworkID = NetManager.I.Client.UnityEventReceiver.AddResponseObserver(HandleSyncMission, false);
			leaveMissionNetworkID = NetManager.I.Client.UnityEventReceiver.AddEventObserver(HandleLeaveMission, false);

            Debug.Log(leaveMissionNetworkID);

            StartCoroutine(MissionUpdater());
        } else {
            SpawnPlayer((string)ndata.Values["id"].ObjectValue, Vector3.zero);
        }
    }

    private void HandleSyncPlayers(byte[] data) {
        NetData ndata = Utils.FromBytesJSON<NetData>(data);

        Player pl = players.Find(x => x.ID.Equals((string)ndata.Values["id"].ObjectValue));
        if (pl != null) {
            Vector3K pos = (Vector3K)ndata.Values["position"].ObjectValue;
            Vector3K rot = (Vector3K)ndata.Values["eulerAngles"].ObjectValue;

            pl.ApplyPosition(pos.FromServerVector3());
            pl.ApplyEulerAngles(rot.FromServerVector3());
        }
    }

    private void HandleShootPlayer(byte[] data) {
        NetData ndata = Utils.FromBytesJSON<NetData>(data);

        Player pl = players.Find(x => x.ID.Equals((string)ndata.Values["id"].ObjectValue));
        if (pl != null) {
            pl.DoShoot((string)ndata.Values["id"].ObjectValue);
        }
    }

    private void HandleSyncMission(byte[] data) {
        NetData ndata = Utils.FromBytesJSON<NetData>(data);

        BaseMission mission = (BaseMission)ndata.Values["mission"].ObjectValue;
        Dictionary<string, BaseMissionObject> objects = mission.Objects;
        MergeDictionary(objects, mission.DynamicObjects);

        //destroy objects on client destroyed on server
        foreach (var o in NetworkObjectDispenser.I.Objects.ToList()) {
            if (!objects.ContainsKey(o.Value.ID)) NetworkObjectDispenser.I.DestroyObject(o.Value.ID);
        }

        //instantiate objects on client created on server
        foreach (var o in objects.ToList()) {
            if (!NetworkObjectDispenser.I.Objects.ContainsKey(o.Value.ID)) {
                SpawnObject(o.Value.ID, o.Value.NameID, o.Value.Position.FromServerVector3(), o.Value.Center.FromServerVector3(), o.Value.Size.FromServerVector3(), o.Value.EulerAngles.FromServerVector3());
            }
        }

        //sync exist objects
        foreach (var o in objects.ToList()) {
            if (NetworkObjectDispenser.I.Objects.ContainsKey(o.Value.ID)) {
                NetworkMissionObject obj = NetworkObjectDispenser.I.Objects[o.Value.ID];
                obj.transform.position = o.Value.Position.FromServerVector3();
                obj.transform.eulerAngles = o.Value.EulerAngles.FromServerVector3();
            }
        }
    }

	private void HandleLeaveMission(byte[] data) {
        NetDataEvent ndata = Utils.FromBytesJSON<NetDataEvent>(data);

		Player pl = players.First(x => x.ID.Equals((string)ndata.Values["id"].ObjectValue));
		players.Remove (pl);
		Destroy (pl.gameObject);
	}

    private void MergeDictionary(Dictionary<string, BaseMissionObject> target, Dictionary<string, BaseMissionObject> source) {
        foreach(BaseMissionObject o in source.Values.ToList()) {
            if(!target.ContainsKey(o.ID)) {
                target.Add(o.ID, o);
            }
        }
    }

    public void SpawnPlayer(string id, Vector3 pos) {
        GameObject pl = Instantiate(playerPrefab);

        pl.transform.position = pos;
        pl.GetComponent<Player>().ID = id;

        players.Add(pl.GetComponent<Player>());
    }

    public void SpawnObject(string id, string nameID, Vector3 pos, Vector3 center, Vector3 size, Vector3 eulerAngles) {
        GameObject obj = NetworkObjectDispenser.I.GetObject(nameID, id);
        obj.transform.position = pos;
        obj.transform.eulerAngles = eulerAngles;

        if (!obj.GetComponent<BoxCollider2D>()) {
            obj.AddComponent<BoxCollider2D>();
        }

        obj.GetComponent<BoxCollider2D>().offset = center;
        obj.GetComponent<BoxCollider2D>().size = size;

        obj.GetComponent<NetworkMissionObject>().ID = id;
    }
}
