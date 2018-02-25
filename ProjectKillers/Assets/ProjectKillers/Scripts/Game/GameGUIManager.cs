using UnityEngine;

public class GameGUIManager : LocalSingletonBehaviour<GameGUIManager> {
    [SerializeField] private HudBar healthBar;

    public void UpdateHealthBar(int health, int maxhealth) {
        healthBar.UpdateBar(health, maxhealth, health.ToString());
    }
}
