using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectKillersServer {
    public class ServerUpdater {
        private Thread updaterThread;
        private static long lastTime = Environment.TickCount;

        public event Action<float> OnUpdate = delegate { };
        public bool CanUpdate = false;

        public static float DeltaTime { get; private set; }

        public ServerUpdater() {
            CanUpdate = true;

            updaterThread = new Thread(UpdateThread);
            updaterThread.Start();
        }

        private void UpdateThread() {
            while (CanUpdate) {
                Thread.Sleep(15);

                var currentTick = Environment.TickCount;
                DeltaTime = (currentTick - lastTime) / 1000.0f;
                lastTime = currentTick;

                OnUpdate.Invoke(DeltaTime);
            }
        }
    }
}
