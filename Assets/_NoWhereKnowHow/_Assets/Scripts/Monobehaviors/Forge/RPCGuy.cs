using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using ForgeAndUnity.Forge;
using UnityEngine.SceneManagement;


namespace CodeNames
{
    public class RPCGuy : CodeNamesGameStateBehavior
    {
        public List<string> cards;
        public Transform playerListBillboard;
        public Transform announcerBillboard;

        public GameObject codeNamesControllerPrefab;
        public PlayersInGame players;
        GameStateApi api;
        NetworkSceneManager _manager;

        bool gameInstanceExists = false;
        GameObject codeNamesObject;

        private void Start()
        {
            if (!NodeManager.IsInitialized || !NodeManager.Instance.IsServer)
            {
                return;
            }
            Debug.Log(gameObject.scene.name);
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
                    networkObject.SendRpc(RPC_SEND_CARD_WORDS_TO_CLIENT, Receivers.OthersBuffered, new object[] { api.Deck.AllCardData });

                    
                    //_manager.Networker.IteratePlayers((player) =>
                    //{
                    //    Debug.Log(player.NetworkId);
                    //});
                    break;

                case GameState.PICK_TEAMS_DONE_BLUE_TO_START:
                case GameState.PICK_TEAMS_DONE_RED_TO_START:
                    //networkObject.SendRpc(RPC_SEND_PLAYER_LIST_TO_CLIENT, Receivers.OthersBuffered, new object[] { players.Players });
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

            DevInputs();
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
                codeNamesObject = (GameObject)Instantiate(codeNamesControllerPrefab, transform.position, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(codeNamesObject, SceneManager.GetSceneByName("Lobby"));

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
                GameObject.Destroy(codeNamesObject);
                gameInstanceExists = false;
                networkObject.SendRpc(RPC_DEACTIVATE_GAME_OBJECTS_ON_CLIENT, Receivers.OthersBuffered);

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
