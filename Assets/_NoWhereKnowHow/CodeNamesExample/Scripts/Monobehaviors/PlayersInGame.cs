using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ForgeAndUnity.Forge;

public class PlayersInGame : MonoBehaviour
{
    NetworkSceneManager _manager;

    public List<Player> online = new List<Player>();

    void Awake()
    {
        if (Environment.GetEnvironmentVariable("PRODUCTION") == "true")
        {
            online.Clear();
        }
        else //development
        {
            online.Clear();
            Add(new Player("Arash", 0));
            Add(new Player("Josh", 1));
            Add(new Player("Brandon", 2));
            Add(new Player("Jessica", 3));
            Add(new Player("Helen", 4));
            Add(new Player("Wei", 5));
            Add(new Player("Bao", 6));
            Add(new Player("Maki", 7));
            Add(new Player("Koikoi", 8));
            Add(new Player("Keikei", 9));
            Add(new Player("Kiki", 10));
            Add(new Player("Lychee", 11));
        }
    }
    public void init()
    {
        if (!NodeManager.IsInitialized || !NodeManager.Instance.IsServer)
        {
            return;
        }

        _manager = NodeManager.Instance.FindNetworkSceneManager(gameObject.scene.name);

        if (_manager == null || !_manager.HasNetworker)
        {
            return;
        }
    }

    public List<Player> Online
    {
        get
        {
            online.Clear();
            if (_manager == null)
            {
                init();
            }


            lock (_manager.Networker.Players)
            {
                // Do your player iteration logic here
                for (int i = 0; i < _manager.Networker.Players.Count; i++)
                {
                    string address = _manager.Networker.Players[i].Ip + ":" + _manager.Networker.Players[i].Port.ToString();
                    if (address == "127.0.0.1:15951")
                        continue;

                    Player p = new Player(address, (int)_manager.Networker.Players[i].NetworkId);
                    Add(p);
                }
            }
            //_manager.Networker.IteratePlayers((player) =>
            //{
            //    Player p = new Player(player.Ip + ":" + player.Port.ToString(), (int)player.NetworkId);
            //    Add(p);
            //    Debug.Log(p.PlayerName);
            //});
            return online;
        }
    }

    public void Add(Player p)
    {
        online.Add(p);
    }


    public string Players{
        get
        {
            List<string> allNames = new List<string>();
            online = Online;
            for (int i = 0; i < online.Count; i++)
            {
                allNames.Add(online[i].PlayerName);
            }
            return string.Join("\n", allNames.ToArray());
        }
    }

    public void ShufflePlayers()
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
