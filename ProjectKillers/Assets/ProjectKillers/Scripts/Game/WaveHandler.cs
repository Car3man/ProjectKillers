using System.Collections;
using System.Collections.Generic;
using Common;
using ProjectKillersCommon;
using TMPro;
using UnityEngine;

public class WaveHandler : LocalSingletonBehaviour<WaveHandler> {
    private string eventID = "";

    [SerializeField] private TextMeshProUGUI waveNumberText;

    public override void DoAwake() {
        eventID = NetManager.I.Client.UnityEventReceiver.AddEventObserver(HandleNewWawe, false);
    }

    private void HandleNewWawe(byte[] data) {
        NetDataEvent netData = Utils.FromBytesJSON<NetDataEvent>(data);

        int waveNumber = (int)netData.Values["wave_number"].ObjectValue;

        waveNumberText.text = string.Format("Wave {0}", waveNumber);

        waveNumberText.GetComponent<TweenScale>().ResetToBeginning();
        waveNumberText.GetComponent<TweenScale>().PlayForward();
    }

    public override void DoDestroy() {
        if (!string.IsNullOrEmpty(eventID)) {
            NetManager.I.Client.UnityEventReceiver.RemoveEventObserver(eventID);
        }
    }
}
