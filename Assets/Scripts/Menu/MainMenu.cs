using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class MainMenu : MonoBehaviour
{
    public static bool UseSteam { get; private set; } = true;
    [SerializeField] GameObject landingPage, joinMenu, lobbyPrefab, networkManagerPrefab;

    Callback<LobbyCreated_t> lobbyCreated;
    Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    Callback<LobbyEnter_t> lobbyEntered;

    public static CSteamID LobbyID { get; private set; }

    private void OnEnable()
    {
        //Connect Steam callbacks
        if (!UseSteam) return;
        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    private void OnDisable()
    {
        if (!UseSteam && lobbyCreated == null) return;
        lobbyCreated.Dispose();
        gameLobbyJoinRequested.Dispose();
        lobbyEntered.Dispose();
    }

    //Invokes when lobby was created
    void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (!UseSteam) return;
        //Check if the lobby was not created successfully
        if(callback.m_eResult != EResult.k_EResultOK)
        {
            //Go offline
            ButtonOffline.GoOffline();
            return;
        }

        //Save the identifier of the lobby
        LobbyID = new CSteamID(callback.m_ulSteamIDLobby);
        //Send to the lobby the information about the host identifier
        SteamMatchmaking.SetLobbyData(LobbyID, "HostAddress", SteamUser.GetSteamID().ToString());
        NetworkManager.singleton.StartHost();
        //Open the lobby menu
        ShowLobbyMenu();
    }

    //Invokes when the request for join lobby was accected
    void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        if(!UseSteam) return;
        //Connect to the lobby
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    //Invokes when the player was entered to the lobby
    void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (!UseSteam) return;
        if (NetworkServer.active) return;

        //If there is no network manager - create one
        if(NetworkManager.singleton == null)
            Instantiate(networkManagerPrefab);

        //Get the identifier of the lobby
        LobbyID = new CSteamID(callback.m_ulSteamIDLobby);
        //Read the information of the host identifier 
        string hostAddress = SteamMatchmaking.GetLobbyData(LobbyID, "HostAddress");
        //Set the address of the connection
        NetworkManager.singleton.networkAddress = hostAddress;
        //Start the client
        NetworkManager.singleton.StartClient();

        //Hide panels
        landingPage.SetActive(false);
        joinMenu.SetActive(false);
    }


    public void HostLobby()
    {
        Instantiate(networkManagerPrefab);
        if (UseSteam)
        {
            //Creating the lobby
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, NetworkManager.singleton.maxConnections);
        }
        else
        {
            //Starting the host
            NetworkManager.singleton.StartHost();

            //Open the lobby menu
            ShowLobbyMenu();
        }
    }

    void ShowLobbyMenu()
    {
        //Hide panels
        joinMenu.SetActive(false);
        landingPage.SetActive(false);

        //Creating the lobby menu
        GameObject lobbyMenu = Instantiate(lobbyPrefab);
        NetworkServer.Spawn(lobbyMenu);
    }

    //Open the panel of joining
    public void ShowJoinMenu()
    {
        Instantiate(networkManagerPrefab);
        joinMenu.SetActive(true);
        landingPage.SetActive(false);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
            Application.Quit();
        #endif
    }
}
