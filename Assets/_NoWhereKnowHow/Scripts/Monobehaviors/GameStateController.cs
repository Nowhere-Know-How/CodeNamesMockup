using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeNames;

namespace CodeNames
{
    // Emits GameStates Events
    public class GameStateController : SingletonBehaviour<GameStateController>
    {
        GameStateManager GM;
        GameState gameState;


        void Start()
        {
            GM = GameStateManager.Instance;
        }

        private void Update()
        {            
            if (Input.GetKeyDown(KeyCode.N))
            {
                EventManager.onGameStateChange.Invoke(gameState);
                Debug.Log("INIT");
            }
            // if (Input.GetKeyDown(KeyCode.X))
            // {
            //     Card card = GM.Deck.DealCard();
            //     Debug.Log("Draw Card. Info: Text -" + card.Text + ", State - " + card.State);
            // }
            // if (Input.GetKeyDown(KeyCode.C))
            // {
            //     GM.Deck.ResetDeck();
            //     Debug.Log("Reset Deck");
            // }
            // if (Input.GetKeyDown(KeyCode.R))
            // {
            //     GM.DrawNewDeck();
            //     Debug.Log("Redraft Deck");
            // }
            // if (Input.GetKeyDown(KeyCode.V))
            // {
            //     int index = 0;
            //     GM.RevealCard(index);
            //     Card card = GM.Deck.DealCard();
            //     Debug.Log("Draw Card. Info: Text -" + card.Text + ", State - " + card.State + ", CardColor - " + GM.KeyCard.GetCardColor(index).ToString());
            // }
        }
    }
}