using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] Transform content;
    [SerializeField] LeaderBoardLine linePrefab;

    Dictionary<PlayerNetworked,LeaderBoardLine> lines = new Dictionary<PlayerNetworked,LeaderBoardLine>();


    private void OnEnable()
    {
        //Subscribe to user information change
        PlayerNetworked.ClientOnInfoUpdated += ClientHandleInfoUpdated;
        //Getting players list
        List<PlayerNetworked> players = ((GameNetworkManager)NetworkManager.singleton).NetworkPlayers;
        //Create line for each player
        foreach (var player in players)
            CreateLine(player);

        //Start coroutine of lobby update
        StartCoroutine(UpdateLobby());
    }

    private void OnDisable()
    {
        //Remove each line as game objects
        foreach(var line in lines.Values)
        {
            line.Remove();
        }
        //Clear lines from dictionary
        lines.Clear();

        //Unsubscribe from user information change
        PlayerNetworked.ClientOnInfoUpdated -= ClientHandleInfoUpdated;
    }

    private void ClientHandleInfoUpdated(PlayerNetworked player)
    {
	//If there is the player in the dictionary
        if (lines.ContainsKey(player))
        {
            //If the player is in the list of players
            if (((GameNetworkManager)NetworkManager.singleton).NetworkPlayers.Contains(player))
            {   
                //Get the line
                LeaderBoardLine line = lines[player];
                //Update each point of statistics
                foreach (var stat in player.Stats)
                    line.UpdateData(stat.Key, stat.Value);
            }
            else
            {
                //Remove the line
                lines[player].Remove();
                //Remove the player from dictionary
                lines.Remove(player);
            }
        }
        else
        {
            //If there is no player in the dictionary, create a new line for him
            CreateLine(player);
        }
    }

    private void CreateLine(PlayerNetworked player)
    {
        string name = player.DisplayName;
        int score = player.Stats[PlayerNetworked.E_Stats.Score];
        int deaths = player.Stats[PlayerNetworked.E_Stats.Death];

        var line = Instantiate(linePrefab, content);
        line.Init(name, score, deaths);
        lines.Add(player, line);
    }
    
    IEnumerator UpdateLobby()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            var players = ((GameNetworkManager)NetworkManager.singleton).NetworkPlayers;
            foreach(var player in players) 
                ClientHandleInfoUpdated(player);
        }
    }
}
