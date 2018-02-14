using System.Collections;

namespace UThen {
    public abstract class UThenYieldInstruction<T> : UThenObject<UThenYieldInstruction<T>>, IEnumerator {
        protected bool KeepWaiting { private get; set; }

        public object Current { get { return null; } }

        public bool MoveNext() {
            return KeepWaiting;
        }

        public void Reset() { }
    }
}