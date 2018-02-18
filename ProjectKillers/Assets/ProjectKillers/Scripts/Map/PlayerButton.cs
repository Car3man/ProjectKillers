using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerButton : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI playerNameText;

    public void Init(string playerName) {
        playerNameText.text = playerName;
    }
}
