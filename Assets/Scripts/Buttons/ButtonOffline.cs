using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ButtonOffline : MonoBehaviour
{
    public static void GoOffline()
    {
        NetworkManager.singleton.ServerChangeScene(NetworkManager.singleton.offlineScene);
        if (NetworkServer.active && NetworkClient.isConnected)
            NetworkManager.singleton.StopHost();
        else
            NetworkManager.singleton.StopClient();

        Destroy(NetworkManager.singleton.gameObject);
    }
}
