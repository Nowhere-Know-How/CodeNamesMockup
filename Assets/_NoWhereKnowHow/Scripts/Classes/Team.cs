using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Team
{
    List<Player> players = new List<Player>();
    Player codeMaster = null;

    public List<Player> Players
    {
        get { return players; }
    }

    public Player CodeMaster
    {
        get { return codeMaster; }
    }

    public void PickCodeMaster()
    {
        int i = UnityEngine.Random.Range(0, players.Count);
        codeMaster = players[i];
    }

    public int AddPlayer(Player player)
    {
        players.Add(player);
        return players.Count;
    }

    public void Clear()
    {
        players.Clear();
        codeMaster = null;
    }

    public string ToString()
    {
        List<string> playerNames = new List<string>();
        for (int i = 0; i < players.Count; i++)
        {
            playerNames.Add(players[i].playerName);
        }
        return String.Join(", ", playerNames.ToArray());
    }
}
