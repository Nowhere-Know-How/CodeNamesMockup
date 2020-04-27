using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayersInGame : MonoBehaviour
{
    public static List<Player> online = new List<Player>();

    void Awake()
    {
        if (Environment.GetEnvironmentVariable("PRODUCTION") == "true")
        {
            online.Clear();
        }
        else //development
        {
            online.Clear();
            Add(new Player("Arash", "male1"));
            Add(new Player("Josh", "female1"));
            Add(new Player("Brandon", "male2"));
            Add(new Player("Jessica", "female2"));
            Add(new Player("Helen", "female3"));
            Add(new Player("Wei", "female4"));
            Add(new Player("Bao", "female1"));
            Add(new Player("Maki", "female1"));
            Add(new Player("Koikoi", "female1"));
            Add(new Player("Keikei", "female1"));
            Add(new Player("Kiki", "female1"));
            Add(new Player("Lychee", "female1"));
        }
    }

    public static void Add(Player p)
    {
        online.Add(p);
    }


    public static string Players{
        get
        {
            List<string> allNames = new List<string>();
            for (int i = 0; i < online.Count; i++)
            {
                allNames.Add(online[i].playerName);
            }
            return string.Join("\n", allNames.ToArray());
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
