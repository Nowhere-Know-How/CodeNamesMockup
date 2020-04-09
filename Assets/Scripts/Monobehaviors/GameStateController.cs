using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeNames;

namespace CodeNames
{
    public class GameStateController : MonoBehaviour
    {
        GameStateManager GM;

        void Start()
        {
            GM = GameStateManager.Instance;
		    GM.OnStateChange += StateChangeHandler;
        }

        public void StateChangeHandler () {
            Debug.Log("Changing state: " + GM.gameState);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log(GM.Deck.Count);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                Card card = GM.Deck.DrawCard();
                Debug.Log(card.Text);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                GM.Deck.ResetDeck();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                GM.DrawNewDeck();
            }
        }
    }
}