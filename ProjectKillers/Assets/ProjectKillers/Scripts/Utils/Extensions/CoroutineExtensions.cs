using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Used for starting coroutines from non-monobehaviour classes.
/// </summary>
public static class CoroutineExtensions {
    private sealed class GlobalObject : GlobalSingletonBehaviour<GlobalObject> { }

    private static void InitGlobalObject() {
        GlobalObject.I.hideFlags = HideFlags.HideAndDontSave;
    }

    public static void RunCoroutine([CanBeNull] this IEnumerator coroutine) {
        InitGlobalObject();

        if (coroutine != null)
            GlobalObject.I.StartCoroutine(coroutine);
    }
}