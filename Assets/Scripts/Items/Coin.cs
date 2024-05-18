using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Coin : TriggerObject
{

    [SerializeField] int score;

    [Command(requiresAuthority = false)]
    public override void OnTriggered(PlayerNetworked player)
    {
        player.Stats[PlayerNetworked.E_Stats.Score] += score;
        NetworkServer.Destroy(gameObject);
    }
}
