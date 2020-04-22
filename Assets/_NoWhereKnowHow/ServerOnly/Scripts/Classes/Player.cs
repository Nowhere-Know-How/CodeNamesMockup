using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string playerName;
    public string prefabString;

    public Player(string name, string prefab)
    {
        playerName = name;
        prefab = prefabString;
    }

    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }

    public string PrefabString
    {
        get { return prefabString; }
        set { prefabString = value; }
    }

}
