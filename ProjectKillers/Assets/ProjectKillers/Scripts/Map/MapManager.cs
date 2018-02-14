using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : LocalSingletonBehaviour<MapManager> {
    public void OnButtonLoadMissionClicked(string missionName) {
        TransitionManager.I.LoadGame();
    }
}
