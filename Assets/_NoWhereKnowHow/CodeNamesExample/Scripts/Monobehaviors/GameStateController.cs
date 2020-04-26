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
        public float waitTimePerCodeMasterTurn = 30f;
        public float waitTimeAfterCodeMasterSubmission = 5f;
        public float waitTimeAfterCodeMasterTimeout = 5f;
        public float waitTimePerTeamTurn = 30f;
        public float waitTimeAfterTeamTimeout = 3f;
        public float waitTimeAfterTeamSubmissionResolution = 3f;

        bool isEventListenersInited = false;
        GameState currentTurn;
        
        Coroutine redTurnCoroutine;
        Coroutine redCodeMasterTurnCoroutine;
        Coroutine blueTurnCoroutine;
        Coroutine blueCodeMasterTurnCoroutine;
        
        void Awake()
        {
            InitEventListeners();
        }
        void Start()
        {
            EventManager.onGameStateChange.Invoke(GameState.INIT);
        }
        public void InitEventListeners()
        {
            if (!isEventListenersInited)
            {
                Debug.Log("GameStateManager Event Listeners Registered");
                EventManager.onGameStateApiDone.AddListener(HandleGameStateApiDone);
                EventManager.onGameStateControllerChange.AddListener(HandleControllerStateChange);
                EventManager.onCodeMasterSubmission.AddListener(HandleCodeMasterSubmission);
                EventManager.onTeamSubmission.AddListener(HandleTeamSubmission);
                isEventListenersInited = true;
            }
        }
        void OnDestroy()
        {
            EventManager.onGameStateApiDone.RemoveListener(HandleGameStateApiDone);
            EventManager.onGameStateControllerChange.RemoveListener(HandleControllerStateChange);
            EventManager.onCodeMasterSubmission.RemoveListener(HandleCodeMasterSubmission);
            EventManager.onTeamSubmission.RemoveListener(HandleTeamSubmission);
        }

        private void HandleTeamSubmission(TeamCardSubmission submission)
        {
            if (currentTurn == GameState.BLUE_TEAM_TURN_START && submission.TeamColor == CardColor.Blue)
            {
                EventManager.onForwardedTeamSubmission.Invoke(submission);
            }
            else if (currentTurn == GameState.RED_TEAM_TURN_START && submission.TeamColor == CardColor.Red)
            {
                EventManager.onForwardedTeamSubmission.Invoke(submission);
            }
            else
            {
                Debug.Log("Ignored Choice submitted by team outside of their turn");
            }
        }

        private void HandleCodeMasterSubmission(Clue clue)
        {
            switch (clue.Team)
            {
                case CardColor.Blue:
                    if (currentTurn == GameState.BLUE_TEAM_TURN_CODEMASTER_START)
                    {
                        Debug.Log("Blue clue submitted: " + clue.ToString());
                        EventManager.onForwardedCodeMasterSubmission.Invoke(clue);
                    }
                    else
                    {
                        Debug.Log("Ignored: Clue submitted by team outside of their turn");
                    }
                    break;

                case CardColor.Red:
                    if (currentTurn == GameState.RED_TEAM_TURN_CODEMASTER_START)
                    {
                        Debug.Log("Red clue submitted: " + clue.ToString());
                        EventManager.onForwardedCodeMasterSubmission.Invoke(clue);
                    }
                    else
                    {
                        Debug.Log("Ignored: Clue submitted by team outside of their turn");
                    }
                    break;

                default:
                    throw new System.NotImplementedException("Clue Not Implemented: " + clue.ToString());
            }
        }


        private void HandleControllerStateChange(GameState controller_gs)
        {
            //Debug.Log("GAME CONTROLLER STATE CHANGED: " + controller_gs.ToString());
            switch (controller_gs)
            {
                case GameState.BLUE_TEAM_TURN_CODEMASTER_START:
                    Debug.Log("Blue Team Code Master Turn Start");
                    currentTurn = controller_gs;
                    blueCodeMasterTurnCoroutine = StartCoroutine(EventManager.DelayInvoke(waitTimePerCodeMasterTurn, EventManager.onGameStateControllerChange, GameState.BLUE_TEAM_TURN_CODEMASTER_TIMEOUT));
                    break;

                case GameState.RED_TEAM_TURN_CODEMASTER_START:
                    Debug.Log("Red Team Code Master Turn Start");
                    currentTurn = controller_gs;
                    redCodeMasterTurnCoroutine = StartCoroutine(EventManager.DelayInvoke(waitTimePerCodeMasterTurn, EventManager.onGameStateControllerChange, GameState.RED_TEAM_TURN_CODEMASTER_TIMEOUT));
                    break;

                case GameState.BLUE_TEAM_TURN_CODEMASTER_TIMEOUT:
                    Debug.Log("Codemaster timed out... Waiting " + waitTimeAfterCodeMasterTimeout.ToString() + " seconds for next turn to resume.");
                    currentTurn = GameState.NULL;
                    StartCoroutine(EventManager.DelayInvoke(waitTimeAfterCodeMasterTimeout, EventManager.onGameStateControllerChange, GameState.RED_TEAM_TURN_CODEMASTER_START));
                    break;

                case GameState.RED_TEAM_TURN_CODEMASTER_TIMEOUT:
                    Debug.Log("Codemaster timed out... Waiting " + waitTimeAfterCodeMasterTimeout.ToString() + " seconds for next turn to resume.");
                    currentTurn = GameState.NULL;
                    StartCoroutine(EventManager.DelayInvoke(waitTimeAfterCodeMasterTimeout, EventManager.onGameStateControllerChange, GameState.BLUE_TEAM_TURN_CODEMASTER_START));
                    break;

                case GameState.BLUE_TEAM_TURN_CODEMASTER_SUBMISSION_DONE:
                    Debug.Log("Codemaster submission received! Waiting " + waitTimeAfterCodeMasterSubmission.ToString() + " seconds for next phase");
                    StopCoroutine(blueCodeMasterTurnCoroutine);
                    currentTurn = GameState.NULL;
                    StartCoroutine(EventManager.DelayInvoke(waitTimeAfterCodeMasterSubmission, EventManager.onGameStateControllerChange, GameState.BLUE_TEAM_TURN_START));
                    break;

                case GameState.RED_TEAM_TURN_CODEMASTER_SUBMISSION_DONE:
                    Debug.Log("Codemaster submission received! Waiting " + waitTimeAfterCodeMasterSubmission.ToString() + " seconds for next phase");
                    StopCoroutine(redCodeMasterTurnCoroutine);
                    currentTurn = GameState.NULL;
                    StartCoroutine(EventManager.DelayInvoke(waitTimeAfterCodeMasterSubmission, EventManager.onGameStateControllerChange, GameState.RED_TEAM_TURN_START));
                    break;

                case GameState.BLUE_TEAM_TURN_START:
                    Debug.Log("Blue Team Start");
                    currentTurn = controller_gs;
                    blueTurnCoroutine = StartCoroutine(EventManager.DelayInvoke(waitTimePerTeamTurn, EventManager.onGameStateControllerChange, GameState.BLUE_TEAM_TURN_TIMEOUT));
                    break;

                case GameState.RED_TEAM_TURN_START:
                    Debug.Log("Red Team Start");
                    currentTurn = controller_gs;
                    redTurnCoroutine = StartCoroutine(EventManager.DelayInvoke(waitTimePerTeamTurn, EventManager.onGameStateControllerChange, GameState.RED_TEAM_TURN_TIMEOUT));
                    break;

                case GameState.BLUE_TEAM_TURN_TIMEOUT:
                    Debug.Log("Blue team turn timed out. Waiting " + waitTimeAfterTeamTimeout.ToString() + " seconds.");
                    StartCoroutine(EventManager.DelayInvoke(waitTimeAfterCodeMasterSubmission, EventManager.onGameStateControllerChange, GameState.RED_TEAM_TURN_CODEMASTER_START));
                    break;

                case GameState.RED_TEAM_TURN_TIMEOUT:
                    Debug.Log("Red team turn timed out. Waiting " + waitTimeAfterTeamTimeout.ToString() + " seconds.");
                    StartCoroutine(EventManager.DelayInvoke(waitTimeAfterCodeMasterSubmission, EventManager.onGameStateControllerChange, GameState.BLUE_TEAM_TURN_CODEMASTER_START));
                    break;

                default:
                    throw new System.NotImplementedException("GameState Not Implemented: " + controller_gs.ToString());
            }
        }

        private void HandleGameStateApiDone(GameState gs)
        {
            //Debug.Log("GAME STATE CHANGED: " + gs.ToString());
            switch (gs)
            {
                case GameState.INIT_DONE:
                    EventManager.onGameStateChange.Invoke(GameState.PICK_TEAMS);
                    break;

                case GameState.PICK_TEAMS_DONE_BLUE_TO_START:
                    Debug.Log("Waiting for teams to meet each other... " + waitTimeAfterTeamPick.ToString() + " seconds");
                    StartCoroutine(EventManager.DelayInvoke(waitTimeAfterTeamPick, EventManager.onGameStateControllerChange, GameState.BLUE_TEAM_TURN_CODEMASTER_START));
                    break;

                case GameState.PICK_TEAMS_DONE_RED_TO_START:
                    Debug.Log("Waiting for teams to meet each other... " + waitTimeAfterTeamPick.ToString() + " seconds");
                    StartCoroutine(EventManager.DelayInvoke(waitTimeAfterTeamPick, EventManager.onGameStateControllerChange, GameState.RED_TEAM_TURN_CODEMASTER_START));
                    break;


                case GameState.BLUE_TEAM_TURN_NO_MORE_GUESSES:
                    StopCoroutine(blueTurnCoroutine);
                    currentTurn = GameState.NULL;
                    Debug.Log("Blue team turn ended. No more guesses! Waiting " + waitTimeAfterTeamSubmissionResolution.ToString() + " seconds.");
                    StartCoroutine(EventManager.DelayInvoke(waitTimeAfterTeamSubmissionResolution, EventManager.onGameStateControllerChange, GameState.RED_TEAM_TURN_CODEMASTER_START));
                    break;

                case GameState.BLUE_TEAM_TURN_END:
                    StopCoroutine(blueTurnCoroutine);
                    currentTurn = GameState.NULL;
                    Debug.Log("Blue team turn ended. Waiting " + waitTimeAfterTeamSubmissionResolution.ToString() + " seconds.");
                    StartCoroutine(EventManager.DelayInvoke(waitTimeAfterTeamSubmissionResolution, EventManager.onGameStateControllerChange, GameState.RED_TEAM_TURN_CODEMASTER_START));
                    break;
                
                case GameState.RED_TEAM_TURN_NO_MORE_GUESSES:
                    StopCoroutine(redTurnCoroutine);
                    currentTurn = GameState.NULL;
                    Debug.Log("Red team turn ended. No more guesses! Waiting " + waitTimeAfterTeamSubmissionResolution.ToString() + " seconds.");
                    StartCoroutine(EventManager.DelayInvoke(waitTimeAfterTeamSubmissionResolution, EventManager.onGameStateControllerChange, GameState.BLUE_TEAM_TURN_CODEMASTER_START));
                    break;

                case GameState.RED_TEAM_TURN_END:
                    StopCoroutine(redTurnCoroutine);
                    currentTurn = GameState.NULL;
                    Debug.Log("Red team turn ended. Waiting " + waitTimeAfterTeamSubmissionResolution.ToString() + " seconds.");
                    StartCoroutine(EventManager.DelayInvoke(waitTimeAfterTeamSubmissionResolution, EventManager.onGameStateControllerChange, GameState.BLUE_TEAM_TURN_CODEMASTER_START));
                    break;

                case GameState.BLUE_TEAM_TURN_CONTINUE:
                    Debug.Log("Blue turn continues! Waiting " + waitTimeAfterTeamSubmissionResolution.ToString() + " seconds for next phase");
                    StopCoroutine(blueTurnCoroutine);
                    currentTurn = GameState.NULL;
                    StartCoroutine(EventManager.DelayInvoke(waitTimeAfterTeamSubmissionResolution, EventManager.onGameStateControllerChange, GameState.BLUE_TEAM_TURN_START));
                    break;

                case GameState.RED_TEAM_TURN_CONTINUE:
                    Debug.Log("Red turn continues! Waiting " + waitTimeAfterTeamSubmissionResolution.ToString() + " seconds for next phase");
                    StopCoroutine(redTurnCoroutine);
                    currentTurn = GameState.NULL;
                    StartCoroutine(EventManager.DelayInvoke(waitTimeAfterTeamSubmissionResolution, EventManager.onGameStateControllerChange, GameState.RED_TEAM_TURN_START));
                    break;

                case GameState.BLUE_TEAM_WINS:
                    if (redTurnCoroutine != null)
                        StopCoroutine(redTurnCoroutine);
                    if (blueTurnCoroutine != null)
                        StopCoroutine(blueTurnCoroutine);
                    currentTurn = GameState.NULL;
                    Debug.Log("Blue Team Wins!");
                    break;

                case GameState.RED_TEAM_WINS:
                    if (redTurnCoroutine != null)
                        StopCoroutine(redTurnCoroutine);
                    if (blueTurnCoroutine != null)
                        StopCoroutine(blueTurnCoroutine);
                    currentTurn = GameState.NULL;
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
                //Debug.Log("Red CodeMaster Submit!");
                Clue clue = new Clue(CardColor.Red, "happy", 1);
                EventManager.onCodeMasterSubmission.Invoke(clue);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                //Debug.Log("Blue CodeMaster Submit!");
                Clue clue = new Clue(CardColor.Blue, "panda", 1);
                EventManager.onCodeMasterSubmission.Invoke(clue);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                //Debug.Log("Red Team Submit");
                TeamCardSubmission submission = new TeamCardSubmission((CardChoice)debugCardIndex, CardColor.Red);
                EventManager.onTeamSubmission.Invoke(submission);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                //Debug.Log("Blue Team Submit");
                TeamCardSubmission submission = new TeamCardSubmission((CardChoice)debugCardIndex, CardColor.Blue);
                EventManager.onTeamSubmission.Invoke(submission);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                //Debug.Log("Blue Team Submit");
                TeamCardSubmission submission = new TeamCardSubmission(CardChoice.PASS, CardColor.Blue);
                EventManager.onTeamSubmission.Invoke(submission);
            }
        }
    }
}