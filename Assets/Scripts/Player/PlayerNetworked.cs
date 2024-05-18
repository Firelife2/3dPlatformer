using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;
using Steamworks;


public class PlayerNetworked : NetworkBehaviour
{
    //Enum of the statistics
    public enum E_Stats
    {
        Score,
        Death
    }
    
    //Actions that broadcast about client information change
    public static event Action<PlayerNetworked> ClientOnInfoUpdated;

    //The dictionary to save the statistics ( Key - an element from enum, Value - a whole number)
    public SyncDictionary<E_Stats, int> Stats { get; } = new SyncDictionary<E_Stats, int>();

    //The field to save the character
    [SyncVar]
    GameObject character;
    public GameObject Character
    {
        get { return character; }
        [Server]
        set { character = value; }
    }

    //The field to save the name of the player
    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
    string displayName;
    public string DisplayName
    {
        get { return displayName; }
        [Server]
        set { displayName = value; }
    }


    //The field to save the SteamID of the lobby
    [SyncVar]ulong steamId;
    public ulong SteamId
    {
        get => steamId;
        set
        { 
            steamId = value;
            CSteamID cSteamId = new CSteamID(steamId);
            displayName = SteamFriends.GetFriendPersonaName(cSteamId);
        }
    }
    public override void OnStartServer()
    {
        //Add statistics
        Stats.Add(E_Stats.Score, 0);
        Stats.Add(E_Stats.Death, 0);
    }


    void Start()
    {
        //Make it not able to destroy on scene load
        DontDestroyOnLoad(gameObject);
    }

    public override void OnStartClient()
    {
        //Connect the callback about statistics update
        Stats.Callback += ClientHandleStatsUpdate;

        //If the player is not a client 
        if (!isClientOnly) return;

        //Add player to the list of players
        ((GameNetworkManager)NetworkManager.singleton).NetworkPlayers.Add(this);
    }

    public override void OnStopClient()
    {
        //If the player is not a client then remove him from the list of players
        if (!isClientOnly)
            ((GameNetworkManager)NetworkManager.singleton).
                NetworkPlayers.Remove(this);

        //Broadcase about the player information change
        ClientOnInfoUpdated?.Invoke(this);
    }


    //Invokes when the dictionary with statistics changed
    private void ClientHandleStatsUpdate(SyncIDictionary<E_Stats, int>.Operation op, E_Stats key, int item)
    {
        ClientOnInfoUpdated?.Invoke(this);
    }

    //Invokes when the display name changed
    void ClientHandleDisplayNameUpdated(string oldName, string newName)
    {
        ClientOnInfoUpdated?.Invoke(this);
    }

}
