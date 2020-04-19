using UnityEngine;
using System.Collections.Generic;
using CodeNames;
using System.Collections;
using System;

namespace CodeNames
{
    public class GameStateManager : SingletonBehaviour<GameStateManager>
    {
        public float waitTimePerTeamTurn = 30f;
        public float waitTimePerCodeMasterTurn = 30f;
        public float waitTimeAfterCodeMasterSubmission = 5f;
        public float waitTimeCardResolution = 5f;
        int minPlayersRequired = 4;

        Deck deck; //The deck is considered the playing field. Unused cards exist in the SQLite DB
        KeyCard keyCard;
        
        int CardsToWinTeamRed;
        Team redTeam = new Team();
        Coroutine redTurnCoroutine;
        Coroutine redCodeMasterTurnCoroutine;

        int CardsToWinTeamBlue;
        Team blueTeam = new Team();
        Coroutine blueTurnCoroutine;
        Coroutine blueCodeMasterTurnCoroutine;

        CardColor teamWithFirstTurn;
        GameState currentTurn;
        Clue lastClue;

        bool isGameDataInited = false;
        bool isEventListenersInited = false;
        

        public bool IsGameDataInited
        {
            get { return isGameDataInited; }
        }
        public CardColor TeamWithFirstTurn
        {
            get { return teamWithFirstTurn; }
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
                EventManager.onGameStateChange.AddListener(HandleStateChange);
                EventManager.onCodeMasterSubmission.AddListener(HandleCodeMasterSubmission);
                EventManager.onTeamSubmission.AddListener(HandleTeamSubmission);
                isEventListenersInited = true;
            }
        }

        private GameStateManager()
        {
        }

        private void HandleTeamSubmission(TeamCardSubmission submission)
        {
            if (currentTurn == GameState.BLUE_TEAM_TURN_START && submission.TeamColor == CardColor.Blue)
            {
                StopCoroutine(blueTurnCoroutine);
                RevealCardResolutions revealResolution = RevealCard(submission.CardIndex);
                HandleRevealCardResolution(revealResolution, submission.TeamColor);
            }
            else if (currentTurn == GameState.RED_TEAM_TURN_START && submission.TeamColor == CardColor.Red)
            {
                StopCoroutine(redTurnCoroutine);
                RevealCardResolutions revealResolution = RevealCard(submission.CardIndex);
                HandleRevealCardResolution(revealResolution, submission.TeamColor);
            }
            else
            {
                Debug.Log("Ignored Choice submitted by team outside of their turn");
            }
        }

        public void HandleRevealCardResolution(RevealCardResolutions revealResolution, CardColor teamColor)
        {
            switch (revealResolution)
            {
                case RevealCardResolutions.ALREADY_REVEALED:
                    Debug.Log("Ignored Submission. Already revealed card was chosen");
                    break;

                case RevealCardResolutions.REVEALED_BLACK_TEAM_CARD:
                    Debug.Log("Black card revealed! " + teamColor.ToString() + " team loses the game");
                    currentTurn = GameState.NULL;
                    if (teamColor == CardColor.Blue)
                    {
                        EventManager.DelayInvoke(waitTimeCardResolution, EventManager.onGameStateChangeDone, GameState.RED_TEAM_WINS);
                    }
                    else if (teamColor == CardColor.Red)
                    {
                        EventManager.DelayInvoke(waitTimeCardResolution, EventManager.onGameStateChangeDone, GameState.BLUE_TEAM_WINS);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("A team must be blue or red");
                    }
                    break;

                case RevealCardResolutions.REVEALED_BROWN_TEAM_CARD:
                    Debug.Log("Brown card revealed! " + teamColor.ToString() + " team loses their turn");
                    currentTurn = GameState.NULL;
                    if (teamColor == CardColor.Blue)
                    {
                        StartCoroutine(EventManager.DelayInvoke(waitTimeCardResolution, EventManager.onGameStateChangeDone, GameState.BLUE_TEAM_TURN_END));
                    }
                    else if (teamColor == CardColor.Red)
                    {
                        StartCoroutine(EventManager.DelayInvoke(waitTimeCardResolution, EventManager.onGameStateChangeDone, GameState.RED_TEAM_TURN_END));
                    }
                    else
                    {
                        throw new System.InvalidOperationException("A team must be blue or red");
                    }
                    break;

                case RevealCardResolutions.REVEALED_BLUE_TEAM_CARD:
                    currentTurn = GameState.NULL;
                    CardsToWinTeamBlue -= 1;
                    PrintScore();
                    if (CardsToWinTeamBlue == 0)
                    {
                        StartCoroutine(EventManager.DelayInvoke(waitTimeCardResolution, EventManager.onGameStateChangeDone, GameState.BLUE_TEAM_WINS));
                    }
                    else
                    {
                        if (teamColor == CardColor.Blue)
                        {
                            Debug.Log("Blue card revealed! " + teamColor.ToString() + " team continues their turn");
                            StartCoroutine(EventManager.DelayInvoke(waitTimeCardResolution, EventManager.onGameStateChangeDone, GameState.BLUE_TEAM_TURN_CONTINUE));
                        }
                        else if (teamColor == CardColor.Red)
                        {
                            Debug.Log("Blue card revealed! " + teamColor.ToString() + " team loses their turn");
                            StartCoroutine(EventManager.DelayInvoke(waitTimeCardResolution, EventManager.onGameStateChangeDone, GameState.RED_TEAM_TURN_END));
                        }
                        else
                        {
                            throw new System.InvalidOperationException("A team must be blue or red");
                        }
                    }
                    break;



                case RevealCardResolutions.REVEALED_RED_TEAM_CARD:
                    currentTurn = GameState.NULL;
                    CardsToWinTeamRed -= 1;
                    PrintScore();
                    if (CardsToWinTeamRed == 0)
                    {
                        StartCoroutine(EventManager.DelayInvoke(waitTimeCardResolution, EventManager.onGameStateChangeDone, GameState.RED_TEAM_WINS));
                    }
                    else
                    {
                        if (teamColor == CardColor.Blue)
                        {
                            Debug.Log("Red card revealed! " + teamColor.ToString() + " team loses their turn");
                            StartCoroutine(EventManager.DelayInvoke(waitTimeCardResolution, EventManager.onGameStateChangeDone, GameState.BLUE_TEAM_TURN_END));
                        }
                        else if (teamColor == CardColor.Red)
                        {
                            Debug.Log("Red card revealed! " + teamColor.ToString() + " team continues their turn");
                            StartCoroutine(EventManager.DelayInvoke(waitTimeCardResolution, EventManager.onGameStateChangeDone, GameState.RED_TEAM_TURN_CONTINUE));
                        }
                        else
                        {
                            throw new System.InvalidOperationException("A team must be blue or red");
                        }
                    }
                    break;

                default:
                    throw new System.NotImplementedException("RevealCardResolutions not implemented: " + revealResolution.ToString());


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
                        StopCoroutine(blueCodeMasterTurnCoroutine);
                        currentTurn = GameState.NULL;
                        lastClue = clue;
                        StartCoroutine(EventManager.DelayInvoke(waitTimeAfterCodeMasterSubmission, EventManager.onGameStateChangeDone, GameState.BLUE_TEAM_TURN_CODEMASTER_SUBMISSION_DONE));
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Clue submitted by team outside of their turn");
                    }
                    break;

                case CardColor.Red:
                    if (currentTurn == GameState.RED_TEAM_TURN_CODEMASTER_START)
                    {
                        Debug.Log("Red clue submitted: " + clue.ToString());
                        StopCoroutine(redCodeMasterTurnCoroutine);
                        currentTurn = GameState.NULL;
                        lastClue = clue;
                        StartCoroutine(EventManager.DelayInvoke(waitTimeAfterCodeMasterSubmission, EventManager.onGameStateChangeDone, GameState.RED_TEAM_TURN_CODEMASTER_SUBMISSION_DONE));
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

        private void HandleStateChange(GameState gs)
        {
            Debug.Log("GAME STATE CHANGED: " + gs.ToString());
            switch (gs)
            {
                #region GAMESTATE INIT
                case GameState.INIT:
                    LoadKeyCardFromDB();
                    LoadDeckFromDB();
                    InitializeScore();
                    isGameDataInited = true;
                    EventManager.onGameStateChangeDone.Invoke(GameState.INIT_DONE);
                    break;
                #endregion

                #region GAMESTATE PICK TEAMS
                case GameState.PICK_TEAMS:
                    Debug.Log("Players Online: " + PlayersInGame.online.Count.ToString());
                    redTeam.Clear();
                    blueTeam.Clear();
                    if (PlayersInGame.online.Count < minPlayersRequired)
                    {
                        throw new System.NotImplementedException("CodeNames needs at least " + minPlayersRequired.ToString() + " people to play");
                    }

                    PlayersInGame.ShufflePlayers();
                    for (int i = 0; i < PlayersInGame.online.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            redTeam.AddPlayer(PlayersInGame.online[i]);
                        }
                        else
                        {
                            blueTeam.AddPlayer(PlayersInGame.online[i]);
                        }
                    }

                    redTeam.PickCodeMaster();
                    blueTeam.PickCodeMaster();

                    Debug.Log("Teams picked");
                    Debug.Log("Red Team: " + redTeam.ToString());
                    Debug.Log("Red CodeMaster: " + redTeam.CodeMaster.PlayerName);
                    Debug.Log("Blue Team: " + blueTeam.ToString());
                    Debug.Log("Blue CodeMaster: " + blueTeam.CodeMaster.PlayerName);
                    if (teamWithFirstTurn == CardColor.Blue)
                    {
                        EventManager.onGameStateChangeDone.Invoke(GameState.PICK_TEAMS_DONE_BLUE_TO_START);
                    }
                    else if (teamWithFirstTurn == CardColor.Red)
                    {
                        EventManager.onGameStateChangeDone.Invoke(GameState.PICK_TEAMS_DONE_RED_TO_START);
                    }
                    else
                    {
                        throw new System.NotSupportedException("A red or blue team must go first");
                    }
                    break;
                #endregion

                #region GAMESTATE BLUE_TEAM_TURN_CODEMASTER_START
                case GameState.BLUE_TEAM_TURN_CODEMASTER_START:
                    currentTurn = gs;
                    blueCodeMasterTurnCoroutine = StartCoroutine(EventManager.DelayInvoke(waitTimePerCodeMasterTurn, EventManager.onGameStateChangeDone, GameState.BLUE_TEAM_TURN_CODEMASTER_TIMEOUT));
                    break;
                #endregion

                #region GAMESTATE RED_TEAM_TURN_CODEMASTER_START
                case GameState.RED_TEAM_TURN_CODEMASTER_START:
                    currentTurn = gs;
                    redCodeMasterTurnCoroutine = StartCoroutine(EventManager.DelayInvoke(waitTimePerCodeMasterTurn, EventManager.onGameStateChangeDone, GameState.RED_TEAM_TURN_CODEMASTER_TIMEOUT));
                    break;
                #endregion

                case GameState.BLUE_TEAM_TURN_START:
                    currentTurn = gs;
                    blueTurnCoroutine = StartCoroutine(EventManager.DelayInvoke(waitTimePerTeamTurn, EventManager.onGameStateChangeDone, GameState.BLUE_TEAM_TURN_TIMEOUT));
                    break;

                case GameState.RED_TEAM_TURN_START:
                    currentTurn = gs;
                    redTurnCoroutine = StartCoroutine(EventManager.DelayInvoke(waitTimePerTeamTurn, EventManager.onGameStateChangeDone, GameState.RED_TEAM_TURN_TIMEOUT));
                    break;

                default:
                    throw new System.NotImplementedException("GameState Not Implemented: " + gs.ToString());
            }
        }


        protected void InitializeScore()
        {
            Debug.Log("Initializing Score...");
            Debug.Assert(keyCard.data.Count == deck.Count);
            CardsToWinTeamRed = 0;
            CardsToWinTeamBlue = 0;

            for (int i = 0; i < keyCard.data.Count; i++)
            {
                if (keyCard.data[i] == CardColor.Red)
                {
                    CardsToWinTeamRed += 1;
                }
                else if (keyCard.data[i] == CardColor.Blue)
                {
                    CardsToWinTeamBlue += 1;
                }
            }

            if (CardsToWinTeamBlue > CardsToWinTeamRed)
            {
                teamWithFirstTurn = CardColor.Blue;
            }
            else if (CardsToWinTeamBlue < CardsToWinTeamRed)
            {
                teamWithFirstTurn = CardColor.Red;
            }
            else
            {
                throw new System.DataMisalignedException("Cards To Win for both teams should never match at the beginning of the game");
            }

            Debug.Log("Cards Left to Win: " + "\nRed - " + CardsToWinTeamRed.ToString() + ", Blue - " + CardsToWinTeamBlue.ToString());
        }

        public RevealCardResolutions RevealCard(int index) //Reveals the card at the index of the deck
        {
            try
            {
                if (deck.Cards[index].State == CardState.Revealed){
                    return RevealCardResolutions.ALREADY_REVEALED;
                }
                else{
                    deck.Cards[index].State = CardState.Revealed;
                    CardColor cardColor = keyCard.data[index];
                    switch (cardColor)
                    {
                        case CardColor.Red:
                            return RevealCardResolutions.REVEALED_RED_TEAM_CARD;
                        case CardColor.Blue:
                            return RevealCardResolutions.REVEALED_BLUE_TEAM_CARD;
                        case CardColor.Brown:
                            return RevealCardResolutions.REVEALED_BROWN_TEAM_CARD;
                        case CardColor.Black:
                            return RevealCardResolutions.REVEALED_BLACK_TEAM_CARD;
                        default:
                            throw new System.Exception("RevealCardResolutions not implemented: " + cardColor);
                    }
                }
            }
            catch
            {
                return RevealCardResolutions.ERROR;
            }

        }

        public KeyCard KeyCard
        {
            get { return keyCard; }
        }
        public void DrawNewDeck()
        {
            LoadDeckFromDB();
        }

        public Deck Deck
        {
            get { return deck; }
        }
        protected void LoadKeyCardFromDB()
        {
            Debug.Log("Drawing KeyCard");
            deck = new Deck();
            keyCard = SqliteApi.GetRandomKeyCard();
        }

        protected void LoadDeckFromDB()
        {
            Debug.Log("Drawing Cards for the Deck");
            deck = new Deck();
            List<string> words = SqliteApi.GetRandomWords();
            for (int i = 0; i < words.Count; i++)
            {
                deck.Add(new Card(words[i]));
            }
            Debug.Log("Words in Deck: " + String.Join(", ", words.ToArray()));
        }

        protected void PrintScore()
        {
            Debug.Log("Cards Left to Win: " + "\nRed - " + CardsToWinTeamRed.ToString() + ", Blue - " + CardsToWinTeamBlue.ToString());
        }
        
    }
}
