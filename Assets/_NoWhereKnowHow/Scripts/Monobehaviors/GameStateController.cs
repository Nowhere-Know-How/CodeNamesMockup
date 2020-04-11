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
		    //GM.OnStateChange += StateChangeHandler;
        }

        public void StateChangeHandler () {
            //Debug.Log("Changing state: " + GM.gameState);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("Cards in Deck: " + GM.Deck.Count.ToString());
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                Card card = GM.Deck.DealCard();
                Debug.Log("Draw Card. Info: Text -" + card.Text + ", State - " + card.State);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                GM.Deck.ResetDeck();
                Debug.Log("Reset Deck");
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                GM.DrawNewDeck();
                Debug.Log("Redraft Deck");
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                int index = 0;
                GM.RevealCard(index);
                Card card = GM.Deck.DealCard();
                Debug.Log("Draw Card. Info: Text -" + card.Text + ", State - " + card.State + ", CardColor - " + GM.KeyCard.GetCardColor(index).ToString());
            }
        }
    }
}