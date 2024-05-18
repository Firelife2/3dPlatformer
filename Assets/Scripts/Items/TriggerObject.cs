using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TriggerObject : NetworkBehaviour
{
    [Command(requiresAuthority = false)]
    public virtual void OnTriggered(PlayerNetworked player){}

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterEntity character))
        {
            var player = character.Owner;
            if (!player.hasAuthority) return;
            OnTriggered(player);
        }
    }
}
