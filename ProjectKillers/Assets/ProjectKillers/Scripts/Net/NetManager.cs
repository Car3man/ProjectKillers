using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : GlobalSingletonBehaviour<NetManager> {
    public UnityClient Client { get; private set; }
    public string ID { get; private set; }

    public event Action OnConnect = delegate { };
    public event Action OnDisconnect = delegate { };

    public void Connect(string ip) {
        ID = Guid.NewGuid().ToString();

        Client = new UnityClient();

        Client.Setup(ip, 6000, "pkillers");
        Client.OnConnectEvent += Client_OnConnectEvent;
        Client.OnDisconnectEvent += Client_OnDisconnectEvent;
        Client.Connect();
    }

    private void Client_OnConnectEvent() {
        OnConnect.Invoke();
    }

    private void Client_OnDisconnectEvent() {
        OnDisconnect.Invoke();
    }

    public override void DoDestroy() {
        base.DoDestroy();

        Client.Disconnect();
    }
}
