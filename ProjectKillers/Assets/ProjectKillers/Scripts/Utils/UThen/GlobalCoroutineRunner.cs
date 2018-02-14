namespace UThen {
    public sealed class GlobalCoroutineRunner : CoroutineRunner<GlobalCoroutineRunner> {
        private void Awake() {
            DontDestroyOnLoad(this);
        }
    }
}