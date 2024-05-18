using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyMenu : NetworkBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] TMP_Dropdown levelDropdown;

    [SyncVar(hook = nameof(ClientHandleSelectLevel))]
    int selectedLevel = 0;

    [SerializeField]
    List<string> sceneNames; 


    private void OnEnable()
    {
        //Add levels to the dropdown list
        levelDropdown.AddOptions(sceneNames);

        //Check if the player is host
        if (NetworkServer.active && NetworkClient.isConnected) 
        {
            //Activate buttons and dropdown list
            startButton.interactable = true;
            levelDropdown.interactable = true;
        }

    }

    [ServerCallback]
    public void ServerHandleSelectLevel()
    {
        //Change the number of selected level
        selectedLevel = levelDropdown.value;
    }

    [ServerCallback]
    public void StartGame()
    {
        //Change the scene
        NetworkManager.singleton.ServerChangeScene($"Level {levelDropdown.value + 1}");
    }

    [ClientCallback]
    private void ClientHandleSelectLevel(int oldLevel, int newLevel)
    {
        if (!isClientOnly) return;
        levelDropdown.value = newLevel;
    }

}
