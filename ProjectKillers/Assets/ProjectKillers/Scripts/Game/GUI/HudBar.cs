using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudBar : MonoBehaviour {
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI text;

    public void UpdateBar(float currentValue, float maxValue) {
        fill.fillAmount = currentValue / maxValue;
    }

    public void UpdateBar(float currentValue, float maxValue, string text) {
        fill.fillAmount = currentValue / maxValue;
        this.text.text = text;
    }
}
