using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using ForgeAndUnity.Forge;
using System;
using BeardedManStudios.Forge.Networking.Unity;

namespace CodeNames
{
    public class RPCGuy : CodeNamesGameStateBehavior
    {
        public List<string> cards;
        public Transform announcerBillboard;

        public GameObject codeNamesControllerPrefab;
        public GameObject playerListPrefab;
        public PlayersInGame players;
        GameStateApi api;
        NetworkSceneManager _manager;

        bool gameInstanceExists = false;
        GameObject codeNamesObject;
        ForgeBillboard billboardNetworkObject;

        private void Start()
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

            EventManager.onGameStateApiDone.AddListener(HandleGameStateApiDone);
        }
        void OnDestroy()
        {
            EventManager.onGameStateApiDone.RemoveListener(HandleGameStateApiDone);
        }

        private void HandleGameStateApiDone(GameState gs)
        {
            //These events should only fire on the server from the game state controller
            switch (gs)
            {
                case GameState.INIT_DONE:
                    api = codeNamesObject.GetComponentInChildren<GameStateApi>();
                    networkObject.SendRpc(RPC_SEND_CARD_WORDS_TO_CLIENT, Receivers.Others, new object[] { api.Deck.AllCardData });
                    break;

                default:
                    Debug.Log("GameState Not Implemented: " + gs.ToString());
                    //throw new Exception("GameState Not Implemented: " + gs.ToString());
                    break; 
            }
        }

        private void Update()
        {
            if (NodeManager.Instance.IsServer)
            {
                return;
            }

            if (Environment.GetEnvironmentVariable("PRODUCTION") != "true")
            {
                DevInputs();
            }
        }

        void DevInputs()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                networkObject.SendRpc(RPC_START_CODE_NAMES_ON_SERVER, Receivers.Server);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                networkObject.SendRpc(RPC_END_CODE_NAMES_ON_SERVER, Receivers.Server);
            }


            //if (Input.GetKeyDown(KeyCode.B))
            //{
            //    networkObject.SendRpc(RPC_SEND_CODEMASTER_CLUE_TO_SERVER, Receivers.All);
            //}
            //if (Input.GetKeyDown(KeyCode.R))
            //{
            //    networkObject.SendRpc(RPC_SEND_CODEMASTER_CLUE_TO_SERVER, Receivers.All);
            //}
        }

        #region RPC-Callbacks
        public override void StartCodeNamesOnServer(RpcArgs pArgs)
        {
            if (!networkObject.IsServer)
            {
                return;
            }

            if (!gameInstanceExists)
            {
                GameObject playerListObject = (GameObject)Instantiate(playerListPrefab, transform.position, Quaternion.identity);
                players = playerListObject.GetComponent<PlayersInGame>();
                codeNamesObject = (GameObject)Instantiate(codeNamesControllerPrefab, transform.position, Quaternion.identity);
                billboardNetworkObject = _manager.InstantiateNetworkBehavior("Billboard", null, announcerBillboard.position, announcerBillboard.rotation) as ForgeBillboard;
                gameInstanceExists = true;
            }
        }

        public override void EndCodeNamesOnServer(RpcArgs pArgs)
        {
            if (!networkObject.IsServer)
            {
                return;
            }

            if (gameInstanceExists)
            {
                GameObject.Destroy(players.gameObject);
                GameObject.Destroy(codeNamesObject);
                billboardNetworkObject.networkObject.Destroy();
                gameInstanceExists = false;
                networkObject.SendRpc(RPC_DEACTIVATE_GAME_OBJECTS_ON_CLIENT, Receivers.Others);

            }
        }

        public override void SendCardWordsToClient(RpcArgs args)
        {
            string card_words = args.GetNext<string>();
            Debug.Log("Card Words: " + card_words);
            string[] cardList = card_words.Split(',');
            cards = new List<string>(cardList);

            for (int i = 0; i < cards.Count; i++)
            {
                string word = cards[i];
                OnCardsChanged e = EventManagerClient.onCardsChangedList[i];
                e.Invoke(word);
            }
        }

        public override void DeactivateGameObjectsOnClient(RpcArgs args)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                string word = "~HideCard~";
                OnCardsChanged e = EventManagerClient.onCardsChangedList[i];
                e.Invoke(word);
            }
        }

        #endregion
    }
}
