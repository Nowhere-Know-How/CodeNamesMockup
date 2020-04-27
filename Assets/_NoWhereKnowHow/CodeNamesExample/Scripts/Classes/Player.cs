using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    string playerName;
    int id;

    public Player(string name, int networkId)
    {
        playerName = name;
        id = networkId;
    }

    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }

    public int PrefabString
    {
        get { return id; }
        set { id = value; }
    }

}
