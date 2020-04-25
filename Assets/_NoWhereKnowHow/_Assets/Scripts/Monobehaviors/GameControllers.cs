using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using ForgeAndUnity.Forge;

namespace CodeNames
{
    public class GameControllers : CodeNamesGameStateBehavior
    {
        public GameObject codeNamesControllerPrefab;
        public GameObject playerListPrefab;

        bool gameInstanceExists = false;
        GameObject codeNames;
        
        public delegate void UnityEvent(string card_data);
        public event UnityEvent OnCardsChanged;

        private void Update()
        {
            if (NodeManager.Instance.IsServer)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("Pushed X");
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
                Instantiate(playerListPrefab, transform.position, Quaternion.identity);
                codeNames = (GameObject)Instantiate(codeNamesControllerPrefab, transform.position, Quaternion.identity);
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
            OnCardsChanged(card_words);
        }

        #endregion
    }
}
