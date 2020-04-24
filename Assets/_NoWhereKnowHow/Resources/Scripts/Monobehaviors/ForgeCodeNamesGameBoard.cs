using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;

namespace CodeNames
{
    public class ForgeCodeNamesGameBoard : CodeNamesGameStateBehavior
    {
        public GameObject codeNamesControllerPrefab;
        GameObject codeNames;
        
        public delegate void UnityEvent(string card_data);
        public event UnityEvent OnCardsChanged;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                networkObject.SendRpc(RPC_START_CODE_NAMES_GAME, Receivers.All);
            }
        }

        #region RPC-Callbacks
        public override void StartCodeNamesGame(RpcArgs pArgs)
        {
            if (!networkObject.IsServer)
            {
                return;
            }

            codeNames = (GameObject)Instantiate(codeNamesControllerPrefab, transform.position, Quaternion.identity);
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
