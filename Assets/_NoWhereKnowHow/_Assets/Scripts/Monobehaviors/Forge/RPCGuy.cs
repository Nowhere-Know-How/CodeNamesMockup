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
        public GameObject codeNamesDisplay;

        public GameObject codeNamesControllerPrefab;
        public GameObject playerListPrefab;
        GameStateApi api;

        bool gameInstanceExists = false;
        GameObject playerListObject;
        GameObject codeNamesObject;

        private void Start()
        {
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
                    //networkObject.SendRpc(RPC_TOGGLE_DISPLAY, Receivers.All, new object[] { true });
                    networkObject.SendRpc(RPC_SET_CARD_WORDS, Receivers.All, new object[] { api.Deck.AllCardData });
                    break;

                default:
                    Debug.Log("GameState Not Implemented: " + gs.ToString());
                    break; 
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
                networkObject.SendRpc(RPC_START_CODE_NAMES, Receivers.Server);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                networkObject.SendRpc(RPC_END_CODE_NAMES, Receivers.All);
            }
        }

        #region RPC-Callbacks
        public override void StartCodeNames(RpcArgs pArgs)
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

        public override void EndCodeNames(RpcArgs pArgs)
        {
            if (!networkObject.IsServer)
            {
                if (cards == null)
                {
                    return;
                }

                for (int i = 0; i < cards.Count; i++)
                {
                    string word = "~HideCard~";
                    OnCardsChanged e = EventManagerClient.onCardsChangedList[i];
                    e.Invoke(word);
                }
                cards = null;
                return;
            }

            if (gameInstanceExists)
            {
                GameObject.Destroy(playerListObject);
                GameObject.Destroy(codeNamesObject);
                gameInstanceExists = false;
            }
        }

        public override void ToggleDisplay(RpcArgs args)
        {
            Debug.Log("RPC ToggleDisplay");
            bool render = args.GetNext<bool>();
            codeNamesDisplay.SetActive(render);

        }


        public override void DrawNewDeck(RpcArgs args)
        {
            Debug.Log("new deck");
        }


        public override void SetCardWords(RpcArgs args)
        {
            Debug.Log("RPC CARD WORDS");
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

        #endregion
    }
}
