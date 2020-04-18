using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeNames;

namespace CodeNames
{
    // Emits GameStates Events
    public class GameStateController : SingletonBehaviour<GameStateController>
    {
        GameState gameState;
        float waitTime = 5f;
        float waitEndTime = 0f;

        private void Start()
        {
            GameStateManager.InitEventListeners();
            EventManager.onGameStateChange.Invoke(gameState);
        }

        private void Update()
        {
            if (gameState == GameState.INIT && GameStateManager.IsGameDataInited)
            {
                gameState = GameState.PICK_TEAMS;
                EventManager.onGameStateChange.Invoke(gameState);
            }
            else if (gameState == GameState.PICK_TEAMS && GameStateManager.IsTeamsPicked)
            {
                gameState = GameState.WAIT_FOR_TEAMS_TO_MEET_EACH_OTHER;
                waitEndTime = Time.time + waitTime;
                EventManager.onGameStateChange.Invoke(gameState);
            }
            else if (gameState == GameState.WAIT_FOR_TEAMS_TO_MEET_EACH_OTHER && Time.time > waitEndTime)
            {
                if (GameStateManager.TeamWithFirstTurn == CardColor.Red)
                {
                    gameState = GameState.RED_TEAM_TURN;
                }
                else
                {
                    gameState = GameState.BLUE_TEAM_TURN;
                }
                EventManager.onGameStateChange.Invoke(gameState);
            }


            //if (Input.GetKeyDown(KeyCode.N))
            //{
            //    Debug.Log("Invoke");
            //    EventManager.onGameStateChange.Invoke(gameState);
            //}
            //if (Input.GetKeyDown(KeyCode.X))
            //{
            //    Card card = GameStateManager.Deck.DealCard();
            //    Debug.Log("Draw Card. Info: Text -" + card.Text + ", State - " + card.State);
            //}
            //if (Input.GetKeyDown(KeyCode.C))
            //{
            //    GameStateManager.Deck.ResetDeck();
            //    Debug.Log("Reset Deck");
            //}
            //if (Input.GetKeyDown(KeyCode.R))
            //{
            //    GameStateManager.DrawNewDeck();
            //    Debug.Log("Redraft Deck");
            //}
            //if (Input.GetKeyDown(KeyCode.V))
            //{
            //    int index = 0;
            //    GameStateManager.RevealCard(index);
            //    Card card = GameStateManager.Deck.DealCard();
            //    Debug.Log("Draw Card. Info: Text -" + card.Text + ", State - " + card.State + ", CardColor - " + GameStateManager.KeyCard.GetCardColor(index).ToString());
            //}
        }
    }
}