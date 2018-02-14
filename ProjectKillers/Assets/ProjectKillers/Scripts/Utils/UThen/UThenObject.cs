using System;
using System.Collections;
using UnityEngine;

namespace UThen {
    public abstract class UThenObject<T> where T : UThenObject<T> {
        private bool _isCoroutineStarted;
        private bool _isGlobal;

        protected Coroutine RunCoroutine( IEnumerator coroutine ) {
            if (coroutine == null) throw new ArgumentNullException("coroutine");

            _isCoroutineStarted = true;

            return _isGlobal ? GlobalCoroutineRunner.RunCoroutine(coroutine) : LocalCoroutineRunner.RunCoroutine(coroutine);
        }

        /// <summary>
        /// Makes the updater persist between scenes.
        /// </summary>
        public T Global() {
            if (_isCoroutineStarted)
                throw new InvalidOperationException(string.Format("Cannot apply the 'global' modifier to {0}. Coroutine has already been started.", typeof(T)));

            _isGlobal = true;
            
            return (T)this;
        }
    }
}