using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayersInGame:MonoBehaviour
{
    public static List<Player> online = new List<Player>();

    private void Start()
    {
        if (Environment.GetEnvironmentVariable("PRODUCTION") == "true")
        {
            online.Clear();
        }
        else //development
        {
            online.Add(new Player("Bob", "male1"));
            online.Add(new Player("Josh", "female1"));
            online.Add(new Player("Brandon", "male2"));
            online.Add(new Player("Jessica", "female2"));
            online.Add(new Player("Helen", "female3"));
            online.Add(new Player("Maki", "female4"));
            online.Add(new Player("Roy", "female1"));
        }
    }

    public static void ShufflePlayers()
    {
        for (int i = 0; i < online.Count; i++)
        {
            Player temp = online[i];
            int randomIndex = UnityEngine.Random.Range(i, online.Count);
            online[i] = online[randomIndex];
            online[randomIndex] = temp;
        }
    }
}
