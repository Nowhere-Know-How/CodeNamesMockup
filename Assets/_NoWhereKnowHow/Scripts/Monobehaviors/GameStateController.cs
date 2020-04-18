using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeNames;

namespace CodeNames
{
    // Emits GameStates Events
    public class GameStateController : SingletonBehaviour<GameStateController>
    {
        bool isEventListenersInited = false;

        private void Start()
        {
            EventManager.onGameStateChange.Invoke(GameState.INIT);
        }
        void Awake()
        {
            InitEventListeners();
        }

        public void InitEventListeners()
        {
            if (!isEventListenersInited)
            {
                Debug.Log("GameStateManager Event Listeners Registered");
                EventManager.onGameStateChangeDone.AddListener(HandleStateChangeDone);
                isEventListenersInited = true;
            }
        }

        private void HandleStateChangeDone(GameState gs)
        {
            Debug.Log("GAME STATE CHANGED: " + gs.ToString());
            switch (gs)
            {
                case GameState.INIT_DONE:
                    EventManager.onGameStateChange.Invoke(GameState.PICK_TEAMS);
                    break;

                case GameState.PICK_TEAMS_DONE:
                    EventManager.onGameStateChange.Invoke(GameState.WAIT_FOR_TEAMS_TO_MEET_EACH_OTHER);
                    break;

                case GameState.WAIT_FOR_TEAMS_TO_MEET_EACH_OTHER_DONE_BLUE_TO_START:
                    EventManager.onGameStateChange.Invoke(GameState.BLUE_TEAM_TURN_START);
                    break;

                case GameState.WAIT_FOR_TEAMS_TO_MEET_EACH_OTHER_DONE_RED_TO_START:
                    EventManager.onGameStateChange.Invoke(GameState.RED_TEAM_TURN_START);
                    break;

                case GameState.BLUE_TEAM_TURN_TIMEOUT:
                case GameState.BLUE_TEAM_TURN_END:
                    EventManager.onGameStateChange.Invoke(GameState.RED_TEAM_TURN_START);
                    break;

                case GameState.RED_TEAM_TURN_TIMEOUT:
                case GameState.RED_TEAM_TURN_END:
                    EventManager.onGameStateChange.Invoke(GameState.BLUE_TEAM_TURN_START);
                    break;

                default:
                    throw new System.NotImplementedException("GameState Not Implemented: " + gs.ToString());
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Red Turn Submit!");
                EventManager.onGameStateChange.Invoke(GameState.RED_TEAM_SUBMISSION);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                Debug.Log("Blue Turn Submit!");
                EventManager.onGameStateChange.Invoke(GameState.BLUE_TEAM_SUBMISSION);
            }
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