using System;
using SwiftKernelUnity.Core;
using UnityEngine;
using Common;
using System.Collections.Generic;

public class UnityClient : UnityPeer {
    public event Action OnConnectEvent = delegate { };
    public event Action OnDisconnectEvent = delegate { };

    protected override void OnConnect() {
        Debug.Log("Connected");

        OnConnectEvent.Invoke();
    }

    protected override void OnDisconnect() {
        Debug.Log("Disconnected");

        OnDisconnectEvent.Invoke();
    }

    protected override void OnEventReceived(byte[] data) {
        
    }

    protected override void OnResponseReceived(byte[] data) {
        
    }
}
