using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : LocalSingletonBehaviour<TransitionManager> {
    public const string SCENE_MAIN_MENU_NAME = "Menu";
    public const string SCENE_GAME_NAME = "Game";
    public const string SCENE_LOBBY_NAME = "Lobby";

    public void LoadMainMenu() {
        SceneManager.LoadScene(SCENE_MAIN_MENU_NAME);
    }

    public void LoadGame() {
        SceneManager.LoadScene(SCENE_GAME_NAME);
    }

    public void LoadLobby() {
        SceneManager.LoadScene(SCENE_LOBBY_NAME);
    }
}
