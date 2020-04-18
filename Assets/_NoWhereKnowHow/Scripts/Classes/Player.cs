using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    string playerName;
    string prefabString;

    public string PlayerName {
        get { return playerName; } 
        set { playerName = value; } 
    }

    public string PrefabString {
        get { return prefabString; } 
        set { prefabString = value; } 
    }
    
}
