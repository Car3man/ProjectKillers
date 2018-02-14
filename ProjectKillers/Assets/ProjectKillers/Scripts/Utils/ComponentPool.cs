using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for creating components in advance to avoid runtime performance hit.
/// </summary>
public class ComponentPool<T> where T : Component {
    private readonly GameObject gameObject;
    private readonly List<T> freeObjects = new List<T>();
    private readonly List<T> usedObjects = new List<T>();

    public ComponentPool(GameObject gameObject, int capacity) {
        if (gameObject == null) throw new ArgumentNullException("gameObject");
        this.gameObject = gameObject;

        for (int i = 0; i < capacity; i++) {
            InstantiateComponent();
        }
    }

    public T Request {
        get {
            if (freeObjects.Count <= 0)
                InstantiateComponent();

            T obj = freeObjects[freeObjects.Count - 1];

            freeObjects.Remove(obj);
            usedObjects.Add(obj);

            return obj;
        }
    }

    /// <summary>
    /// Should be called when a component is no longer needed.
    /// </summary>
    public void Free(T obj) {
        if (obj == null) throw new ArgumentNullException("obj");

        if (!freeObjects.Contains(obj))
            freeObjects.Add(obj);

        if (usedObjects.Contains(obj))
            usedObjects.Remove(obj);
    }

    private void InstantiateComponent() {
        var component = gameObject.AddComponent<T>();

        freeObjects.Add(component);
    }
}