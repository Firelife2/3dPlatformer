using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System;

public class JoinMenu : MonoBehaviour
{
    [SerializeField] GameObject lobbyMenu;
    [SerializeField] TMP_InputField inputField;

    private void OnEnable()
    {
        GameNetworkManager.ClientOnConnected += HandleClientConnected;
    }

    private void OnDisable()
    {
        GameNetworkManager.ClientOnConnected -= HandleClientConnected;
    }

    private void HandleClientConnected()
    {
        gameObject.SetActive(false);
    }

    public void Join()
    {
        NetworkManager.singleton.networkAddress = inputField.text;
        NetworkManager.singleton.StartClient();
    }
}
