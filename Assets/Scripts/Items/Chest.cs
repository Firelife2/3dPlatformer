using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Chest : Interactable
{
    [SerializeField] Transform cap;
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
        cap.localRotation = Quaternion.Euler(activated ? activatedRotation : disabledRotation);
    }
}
