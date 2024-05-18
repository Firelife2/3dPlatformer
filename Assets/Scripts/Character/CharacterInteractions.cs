using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class CharacterInteractions : NetworkBehaviour
{
    public Action<PlayerNetworked> ClientInteract;
    [SerializeField] CharacterInput input;
    [SerializeField] CharacterEntity character;
    
    public bool HasInteractions { get; private set; }

    public override void OnStartClient()
    {
        if (!hasAuthority) 
            enabled = false;
    }


    private void Update()
    {
        HasInteractions = ClientInteract != null;
        if (input.Interact) ClientInteract?.Invoke(character.Owner);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            ClientInteract += interactable.Interact;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            ClientInteract -= interactable.Interact;
        }
    }
}
