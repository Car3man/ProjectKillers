using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObjectDispenser : LocalSingletonBehaviour<NetworkObjectDispenser> {
    public Dictionary<string, NetworkMissionObject> Objects = new Dictionary<string, NetworkMissionObject>();
    private List<string> destroyedObjects = new List<string>();

    public GameObject GetObject(string nameID, string id) {
        GameObject obj = Instantiate(Resources.Load<GameObject>(string.Format("NetworkObjectPrefabs/{0}", nameID)));
        if (!obj) throw new ArgumentNullException("obj");
        Objects.Add(id, obj.GetComponent<NetworkMissionObject>());
        return obj;
    }

    public void DestroyObject(string id) {
        StartCoroutine(DelayDestroyObject(id));
    }

    private IEnumerator DelayDestroyObject(string id) {
        if (destroyedObjects.Contains(id)) yield break;
       
        yield return new WaitForSeconds(0.01f);
        if (Objects.ContainsKey(id)) {
            NetworkMissionObject obj = Objects[id];

            Objects.Remove(id);
            Destroy(obj.gameObject);

            destroyedObjects.Add(id);
        }
    }
}
