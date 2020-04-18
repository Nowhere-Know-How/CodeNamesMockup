﻿using System.Collections;
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

                case GameState.RED_TEAM_TURN:
                    Debug.Log("Red Team Turn");
                    break;

                case GameState.BLUE_TEAM_TURN:
                    Debug.Log("Blue Team Turn");
                    break;

                default:
                    throw new System.NotImplementedException("GameState Not Implemented: " + gs.ToString());
            }
        }

        private void Update()
        {
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