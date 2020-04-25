using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using ForgeAndUnity.Forge;
using CodeNames;

namespace CodeNames
{
    public class RPCGuy : CodeNamesGameStateBehavior
    {
        public List<string> cards;

        public GameObject codeNamesControllerPrefab;
        public GameObject playerListPrefab;
        GameStateApi api;

        bool gameInstanceExists = false;
        GameObject playerListObject;
        GameObject codeNamesObject;
        
        //public delegate void UnityEvent(string card_data);
        //public event UnityEvent OnCardsChanged;

        private void Start()
        {
            EventManager.onGameStateApiDone.AddListener(HandleGameStateApiDone);
        }

        private void HandleGameStateApiDone(GameState gs)
        {
            //These events should only fire on the server from the game state controller
            switch (gs)
            {
                case GameState.INIT_DONE:
                    api = codeNamesObject.GetComponentInChildren<GameStateApi>();
                    networkObject.SendRpc(RPC_SET_CARD_WORDS, Receivers.All, new object[] { api.Deck.AllCardData });
                    break;

                default:
                    throw new System.NotImplementedException("GameState Not Implemented: " + gs.ToString());
            }
        }

        private void Update()
        {
            if (NodeManager.Instance.IsServer)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                networkObject.SendRpc(RPC_START_CODE_NAMES_GAME, Receivers.All);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {

            }
        }

        #region RPC-Callbacks
        public override void StartCodeNamesGame(RpcArgs pArgs)
        {
            if (!networkObject.IsServer)
            {
                return;
            }

            if (!gameInstanceExists)
            {
                playerListObject = Instantiate(playerListPrefab, transform.position, Quaternion.identity);
                codeNamesObject = (GameObject)Instantiate(codeNamesControllerPrefab, transform.position, Quaternion.identity);
                gameInstanceExists = true;
            }
        }

        public override void DrawNewDeck(RpcArgs pArgs)
        {
            Debug.Log("new deck");
        }

        public override void SetCardWords(RpcArgs args)
        {
            string card_words = args.GetNext<string>();
            Debug.Log("Card Words: " + card_words);
            string[] cardList = card_words.Split(',');
            cards = new List<string>(cardList);
            //OnCardsChanged(card_words);
        }

        #endregion
    }
}
