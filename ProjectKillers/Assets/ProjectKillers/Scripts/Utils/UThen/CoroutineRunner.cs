using System.Collections;
using UnityEngine;

namespace UThen {
    public abstract class CoroutineRunner<T> : MonoBehaviour where T : CoroutineRunner<T> {
        private static T _instance;

        private static T Instance {
            get {
                if (_instance != null) return _instance;

                var go = new GameObject("GlobalCoroutineRunner") {hideFlags = HideFlags.HideAndDontSave};

                _instance = go.AddComponent<T>();

                return _instance;
            }
        }

        public static Coroutine RunCoroutine( IEnumerator coroutine ) {
            return Instance.StartCoroutine(coroutine);
        }
    }
}