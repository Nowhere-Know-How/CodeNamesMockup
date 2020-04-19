using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeNames;

namespace CodeNames
{
    // Emits GameStates Events
    public class GameStateController : SingletonBehaviour<GameStateController>
    {
        public float waitTimeAfterTeamPick = 5f;

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

                case GameState.PICK_TEAMS_DONE_BLUE_TO_START:
                    Debug.Log("Waiting for teams to meet each other... " + waitTimeAfterTeamPick.ToString() + " seconds");
                    StartCoroutine(EventManager.DelayInvoke(waitTimeAfterTeamPick, EventManager.onGameStateChange, GameState.BLUE_TEAM_TURN_CODEMASTER_START));
                    //EventManager.onGameStateChange.Invoke(GameState.BLUE_TEAM_TURN_CODEMASTER_START);
                    break;

                case GameState.PICK_TEAMS_DONE_RED_TO_START:
                    Debug.Log("Waiting for teams to meet each other... " + waitTimeAfterTeamPick.ToString() + " seconds");
                    StartCoroutine(EventManager.DelayInvoke(waitTimeAfterTeamPick, EventManager.onGameStateChange, GameState.RED_TEAM_TURN_CODEMASTER_START));
                    //EventManager.onGameStateChange.Invoke(GameState.RED_TEAM_TURN_CODEMASTER_START);
                    break;

                case GameState.BLUE_TEAM_TURN_CODEMASTER_TIMEOUT:
                    EventManager.onGameStateChange.Invoke(GameState.RED_TEAM_TURN_CODEMASTER_START);
                    break;

                case GameState.BLUE_TEAM_TURN_CONTINUE:
                case GameState.BLUE_TEAM_TURN_CODEMASTER_SUBMISSION_DONE:
                    EventManager.onGameStateChange.Invoke(GameState.BLUE_TEAM_TURN_START);
                    break;

                case GameState.BLUE_TEAM_TURN_TIMEOUT:
                case GameState.BLUE_TEAM_TURN_END:
                    EventManager.onGameStateChange.Invoke(GameState.RED_TEAM_TURN_CODEMASTER_START);
                    break;

                case GameState.RED_TEAM_TURN_CODEMASTER_TIMEOUT:
                    EventManager.onGameStateChange.Invoke(GameState.BLUE_TEAM_TURN_CODEMASTER_START);
                    break;

                case GameState.RED_TEAM_TURN_CONTINUE:
                case GameState.RED_TEAM_TURN_CODEMASTER_SUBMISSION_DONE:
                    EventManager.onGameStateChange.Invoke(GameState.RED_TEAM_TURN_START);
                    break;

                case GameState.RED_TEAM_TURN_TIMEOUT:
                case GameState.RED_TEAM_TURN_END:
                    EventManager.onGameStateChange.Invoke(GameState.BLUE_TEAM_TURN_CODEMASTER_START);
                    break;

                case GameState.BLUE_TEAM_WINS:
                    Debug.Log("Blue Team Wins!");
                    break;

                case GameState.RED_TEAM_WINS:
                    Debug.Log("Red Team Wins!");
                    break;

                default:
                    throw new System.NotImplementedException("GameState Not Implemented: " + gs.ToString());
            }
        }

        [Range(0, 24)]
        public int debugCardIndex = 0;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Red CodeMaster Submit!");
                Clue clue = new Clue(CardColor.Red, "happy", 3);
                EventManager.onCodeMasterSubmission.Invoke(clue);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                Debug.Log("Blue CodeMaster Submit!");
                Clue clue = new Clue(CardColor.Blue, "happy", 3);
                EventManager.onCodeMasterSubmission.Invoke(clue);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("Red Team Submit");
                TeamCardSubmission submission = new TeamCardSubmission(debugCardIndex, CardColor.Red);
                EventManager.onTeamSubmission.Invoke(submission);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                Debug.Log("Blue Team Submit");
                TeamCardSubmission submission = new TeamCardSubmission(debugCardIndex, CardColor.Blue);
                EventManager.onTeamSubmission.Invoke(submission);
            }
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