using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterEntity : NetworkBehaviour, IHealth
{
    public static Action<PlayerNetworked> ServerCharacterDied;

    [SyncVar]
    private PlayerNetworked owner;
    public PlayerNetworked Owner
    {
        get { return owner; }
        [Server]
        set { owner = value; }
    }

    [ServerCallback]
    public void TakeDamage()
    {
        NetworkServer.Destroy(gameObject);
        ServerCharacterDied?.Invoke(Owner);
    }
}
