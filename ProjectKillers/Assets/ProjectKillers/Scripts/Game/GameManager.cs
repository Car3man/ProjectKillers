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
        NetManager.I.Client.SendRequest(Utils.ToBytesJSON(new NetDataRequest(RequestTypes.EnterInMission, new Dictionary<string, ObjectWrapper>() { { "id", new ObjectWrapper<string>(NetManager.I.ID) } })), requestID);
    }

    private void OnEnterMission(byte[] data) {
        NetDataRequest ndata = Utils.FromBytesJSON<NetDataRequest>(data);

        syncMissionNetworkID = NetManager.I.Client.UnityEventReceiver.AddEventObserver(HandleSyncMission, false);
        shootPlayerNetworkID = NetManager.I.Client.UnityEventReceiver.AddResponseObserver(HandleShootPlayer, false);
        leaveMissionNetworkID = NetManager.I.Client.UnityEventReceiver.AddEventObserver(HandleLeaveMission, false);
    }

    private void HandleShootPlayer(byte[] data) {
        NetDataRequest ndata = Utils.FromBytesJSON<NetDataRequest>(data);

        Player pl = players.Find(x => x.ID.Equals((string)ndata.Values["id"].ObjectValue));
        if (pl != null) {
            pl.DoShoot((string)ndata.Values["id"].ObjectValue);
        }
    }

    private void HandleSyncMission(byte[] data) {
        NetDataEvent ndata = Utils.FromBytesJSON<NetDataEvent>(data);

        BaseMission mission = (BaseMission)ndata.Values["mission"].ObjectValue;

        if (mission.Objects == null) mission.Objects = new Dictionary<string, BaseMissionObject>();
        if (mission.DynamicObjects == null) mission.DynamicObjects = new Dictionary<string, BaseMissionObject>();

        Dictionary<string, BaseMissionObject> objects = mission.Objects;
        MergeDictionary(objects, mission.DynamicObjects);

        //instantiate objects on client created on server
        foreach (var o in objects.ToList()) {
            if (o.Value.Destroyed) continue;

            bool isOwn = o.Value.OwnerID.Equals(NetManager.I.ID);

            if (!NetworkObjectDispenser.I.Objects.ContainsKey(o.Value.ID)) {
                GameObject go = SpawnObject(o.Value.ID, o.Value.NameID, o.Value.Position.FromServerVector3(), o.Value.Center.FromServerVector3(), o.Value.Size.FromServerVector3(), o.Value.EulerAngles.FromServerVector3(), isOwn);
            
                if(o.Value is PlayerObject) {
                    players.Add(go.GetComponent<Player>());

                    if (isOwn) {
                        CameraController.I.Target = go;
                    }
                }
            }
        }

        //sync exist objects
        foreach (var o in objects.ToList()) {
            if (NetworkObjectDispenser.I.Objects.ContainsKey(o.Value.ID)) {
                NetworkMissionObject obj = NetworkObjectDispenser.I.Objects[o.Value.ID];
                obj.SyncTransform(o.Value.Position.FromServerVector3(), o.Value.EulerAngles.FromServerVector3());

                if (o.Value is IHuman) {
                    (obj as IHumanObject).SyncHealth((o.Value as IHuman).Health, (o.Value as IHuman).MaxHealth);

                    if (o.Value is PlayerObject) {
                        GameGUIManager.I.UpdateHealthBar((o.Value as IHuman).Health, (o.Value as IHuman).MaxHealth);
                    }
                }

                if (o.Value.Destroyed) {
                    NetworkObjectDispenser.I.DestroyObject(o.Value.ID);
                }
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

    public GameObject SpawnObject(string id, string nameID, Vector3 pos, Vector3 center, Vector3 size, Vector3 eulerAngles, bool isOwn) {
        GameObject obj = NetworkObjectDispenser.I.GetObject(nameID, id);
        obj.transform.position = pos;
        obj.transform.eulerAngles = eulerAngles;

        if (!obj.GetComponent<BoxCollider2D>()) {
            obj.AddComponent<BoxCollider2D>();
        }

        obj.GetComponent<BoxCollider2D>().offset = center;
        obj.GetComponent<BoxCollider2D>().size = size;
        obj.GetComponent<NetworkMissionObject>().IsOwn = isOwn;

        obj.GetComponent<NetworkMissionObject>().InitID(id);

        return obj;
    }
}
