using Common;
using ProjectKillersCommon;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : LocalSingletonBehaviour<MenuManager> {
    [SerializeField] private TextMeshProUGUI connectText;
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private Button enterButton;

    private void Start() {
        connectText.text = "Connecting...";
        enterButton.interactable = false;

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

        connectText.text = "Connected, enter nickname and enter";
        enterButton.interactable = true;

        NetManager.I.OnConnect -= OnConnect;
        NetManager.I.OnDisconnect -= OnDisconnect;
    }

    private void OnDisconnect() {
        connectText.text = "Connect error, restart game";
        enterButton.interactable = false;

        NetManager.I.OnConnect -= OnConnect;
        NetManager.I.OnDisconnect -= OnDisconnect;
    }

    public void OnButtonEnterClick() {
        if (string.IsNullOrEmpty(nicknameInput.text)) return;

        NetDataRequest loginData = new NetDataRequest(RequestTypes.Login, new Dictionary<string, ObjectWrapper>() { { "nickname", new ObjectWrapper<string>(nicknameInput.text) } });
        string loginID = NetManager.I.Client.UnityEventReceiver.AddResponseObserver(OnLoginResponse, true);
        NetManager.I.Client.SendRequest(Utils.ToBytesJSON(loginData), loginID);
    }

    private void OnLoginResponse(byte[] data) {
        NetDataRequest response = Utils.FromBytesJSON<NetDataRequest>(data);

        switch(response.Result) {
            case RequestResult.Ok:
                TransitionManager.I.LoadLobby();
                break;
            case RequestResult.NicknameOccupied:
                connectText.text = "Nickname occupied";
                break;
            default:
                connectText.text = "Unknow login error";
                break;
        }
    }
}
