using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SceneManagement;
using kcp2k;
using Mirror.FizzySteam;
using Steamworks;

public class GameNetworkManager : NetworkManager
{
    public static event Action ClientOnConnected;

    [SerializeField] GameObject gameManagerPrefab;

    public List<PlayerNetworked> NetworkPlayers = new List<PlayerNetworked>();

    
    public override void Awake()
    {
        //Change the protocol of data-sender
        if (MainMenu.UseSteam)
        {
            transport = GetComponent<FizzySteamworks>();
        }
        else
        {
            transport = GetComponent<KcpTransport>();
        }
        base.Awake();
    }

    //Invokes when a player was connected to the server 
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        //Creating a player
        GameObject playerInstance = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, playerInstance);

        var player = playerInstance.GetComponent<PlayerNetworked>();
        NetworkPlayers.Add(player);

        //Set its username
        if(!MainMenu.UseSteam)
            player.DisplayName = $"Player {conn.connectionId}";
        else
        {
            CSteamID cSteamID = SteamMatchmaking.GetLobbyMemberByIndex(MainMenu.LobbyID,numPlayers - 1);
            player.SteamId = cSteamID.m_SteamID;
        }
    }

    //Invokes when a scene was changed
    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("Level"))
        {
            GameObject gameManager = Instantiate(gameManagerPrefab);
            NetworkServer.Spawn(gameManager);
        }
    }

    //Invokes when a player was disconnected
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        var player = conn.identity.GetComponent<PlayerNetworked>();
        NetworkPlayers.Remove(player);
        base.OnServerDisconnect(conn);
    }

    //Invokes when the server was stopped
    public override void OnStopServer()
    {
        NetworkPlayers.Clear();
    }

    //Invokes in client when was connected to the server
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        ClientOnConnected?.Invoke();
    }

    //Invokes in client when server was disconnected
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        SceneManager.LoadScene("Menu");
        Destroy(gameObject);
    }
}
