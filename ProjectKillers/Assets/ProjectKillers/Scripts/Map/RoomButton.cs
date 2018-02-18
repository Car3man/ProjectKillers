using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomButton : MonoBehaviour {
    public string ID;

    [SerializeField] private string RoomName;
    [SerializeField] private TextMeshProUGUI roomNameText;

    public void Init(string roomname) {
        RoomName = roomname;
        roomNameText.text = roomname;
    }

    public void OnButtonClick() {
        MapManager.I.OnButtonRoomClicked(ID);
    }
}
