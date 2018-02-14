using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObjectDispenser : LocalSingletonBehaviour<NetworkObjectDispenser> {
    public Dictionary<string, NetworkMissionObject> Objects = new Dictionary<string, NetworkMissionObject>();

    public GameObject GetObject(string nameID, string id) {
        GameObject obj = Instantiate(Resources.Load<GameObject>(string.Format("NetworkObjectPrefabs/{0}", nameID)));
        if (!obj) throw new ArgumentNullException("obj");
        Objects.Add(id, obj.GetComponent<NetworkMissionObject>());
        return obj;
    }

    public void DestroyObject(string id) {
        if(Objects.ContainsKey(id)) {
            NetworkMissionObject obj = Objects[id];

            Objects.Remove(id);
            Destroy(obj.gameObject);
        }
    }
}
