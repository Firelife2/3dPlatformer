using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class Interactable : NetworkBehaviour, IInteractable
{
    [SerializeField] UnityEvent actions;

    public virtual void Interact(PlayerNetworked player)
    {
        CmdInteract();
    }

    [Command(requiresAuthority = false)]
    public virtual void CmdInteract()
    {
        actions.Invoke();
    }
}
