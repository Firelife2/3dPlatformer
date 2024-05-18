using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class Lever : Interactable
{
    
    [SerializeField] Transform handle;
    [SerializeField] Vector3 activatedRotation;
    [SerializeField] Vector3 disabledRotation;
    
    [SyncVar(hook = nameof(ClientHandleActiveChanged))]
    bool activated = false;

    [Command(requiresAuthority = false)]
    public override void CmdInteract()
    {
        base.CmdInteract();
        activated = !activated;
    }

    void ClientHandleActiveChanged(bool oldState, bool newState)
    {
        handle.localRotation = Quaternion.Euler(activated ? activatedRotation : disabledRotation);
    }



}
