using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EndDoor : NetworkBehaviour
{
    [SerializeField] GameObject lockedVariant, openedVariant;

    [SyncVar(hook =nameof(ClientHandleLockChange))]
    [SerializeField] bool locked = true;

    
    [Server]
    public void OpenOrClose()
    {
        locked = !locked;
    }


    private void ClientHandleLockChange(bool oldState, bool newState)
    {
        lockedVariant.SetActive(locked);
        openedVariant.SetActive(!locked);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (locked) return;
        if(other.TryGetComponent(out CharacterEntity character))
        {
            character.Owner.Stats[PlayerNetworked.E_Stats.Score] += 100;
            GameManager.Instance.EndGame();
        }
    }
}
