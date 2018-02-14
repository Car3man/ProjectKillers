using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : LocalSingletonBehaviour<MenuManager> {
    [SerializeField] private TextMeshProUGUI connectText;
    [SerializeField] private Button playButton;

    private void Start() {
        connectText.text = "Connecting...";
        playButton.interactable = false;

        string ip = "localhost";
        string configPath = Path.Combine(Application.dataPath, "config.ini");

        if(File.Exists(configPath)) {
            FileStream fs = new FileStream(configPath, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            ip = sr.ReadLine();
            sr.Close();
            fs.Close();
        }

        NetManager.I.OnConnect += OnConnect;
        NetManager.I.OnDisconnect += OnDisconnect;
        NetManager.I.Connect(ip);
    }

    private void OnConnect() {
        StackTrace stackTrace = new StackTrace();

        connectText.text = "Connected";
        playButton.interactable = true;

        NetManager.I.OnConnect -= OnConnect;
        NetManager.I.OnDisconnect -= OnDisconnect;
    }

    private void OnDisconnect() {
        connectText.text = "Connect error";
        playButton.interactable = true;

        NetManager.I.OnConnect -= OnConnect;
        NetManager.I.OnDisconnect -= OnDisconnect;
    }

    public void OnButtonPlayClicked() {
        TransitionManager.I.LoadMap();
    }
}
