using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] GameObject characterPrefab;
    [SerializeField] GameObject lobbyPrefab;
    Transform respawnPosition;

    public override void OnStartServer()
    {
        CharacterEntity.ServerCharacterDied += ServerHandleCharacterDeath;
        respawnPosition = GameObject.FindWithTag("Respawn").transform;
        foreach (var player in ((GameNetworkManager)NetworkManager.singleton).NetworkPlayers)
        {
            SpawnCharacter(player);
        }
    }

    public override void OnStopServer()
    {
        CharacterEntity.ServerCharacterDied -= ServerHandleCharacterDeath;
    }

    [Server]
    void SpawnCharacter(PlayerNetworked player)
    {
        var character = Instantiate(characterPrefab, respawnPosition.position, Quaternion.identity);
        NetworkServer.Spawn(character, player.gameObject);
        player.Character = character;
        character.GetComponent<CharacterEntity>().Owner = player;
    }

    [Server]
    void ServerHandleCharacterDeath(PlayerNetworked player)
    {
        player.Stats[PlayerNetworked.E_Stats.Death]++;
        SpawnCharacter(player);
    }

    [Server]
    public void EndGame()
    {
        foreach(var player in ((GameNetworkManager)NetworkManager.singleton).NetworkPlayers)
        {
            Destroy(player.Character);
        }
        GameObject lobbyMenu = Instantiate(lobbyPrefab);
        NetworkServer.Spawn(lobbyMenu);
    }

    private void Start()
    {
        Instance = this;
    }

}
