using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class KillingObject : TriggerObject
{
    [Command(requiresAuthority = false)]
    public override void OnTriggered(PlayerNetworked player)
    {
        player.Character.GetComponent<IHealth>().TakeDamage();
    }
}
