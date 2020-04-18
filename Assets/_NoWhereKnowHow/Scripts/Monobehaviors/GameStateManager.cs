using UnityEngine;
using System.Collections.Generic;
using CodeNames;
using System.Collections;

namespace CodeNames
{
    public class GameStateManager : SingletonBehaviour<GameStateManager>
    {
        public float waitTimeAfterTeamPick = 5f;
        public float waitTimePerTeamTurn = 5f;
        int minPlayersRequired = 4;

        Deck deck; //The deck is considered the playing field. Unused cards exist in the SQLite DB
        KeyCard keyCard;
        
        int CardsToWinTeamRed;
        Team redTeam = new Team();
        Coroutine redTurnCoroutine;

        int CardsToWinTeamBlue;
        Team blueTeam = new Team();
        Coroutine blueTurnCoroutine;

        CardColor teamWithFirstTurn;
        GameState currentTurn;

        bool isGameDataInited = false;
        bool isEventListenersInited = false;
        bool isTeamsPicked = false;
        

        public bool IsGameDataInited
        {
            get { return isGameDataInited; }
        }
        public bool IsTeamsPicked
        {
            get { return isTeamsPicked; }
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
                isEventListenersInited = true;
            }
        }

        private GameStateManager()
        {
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
                    isTeamsPicked = true;
                    EventManager.onGameStateChangeDone.Invoke(GameState.PICK_TEAMS_DONE);
                    break;
                #endregion

                #region GAMESTATE WAIT_FOR_TEAMS_TO_MEET_EACH_OTHER
                case GameState.WAIT_FOR_TEAMS_TO_MEET_EACH_OTHER:
                    Debug.Log("Game State Manager is waiting... ");
                    if (teamWithFirstTurn == CardColor.Blue) 
                    {
                        StartCoroutine(EventManager.DelayInvoke(waitTimeAfterTeamPick, EventManager.onGameStateChangeDone, GameState.WAIT_FOR_TEAMS_TO_MEET_EACH_OTHER_DONE_BLUE_TO_START));
                    }
                    else if (teamWithFirstTurn == CardColor.Red)
                    {
                        StartCoroutine(EventManager.DelayInvoke(waitTimeAfterTeamPick, EventManager.onGameStateChangeDone, GameState.WAIT_FOR_TEAMS_TO_MEET_EACH_OTHER_DONE_RED_TO_START));
                    }
                    else
                    {
                        throw new System.NotSupportedException("A red or blue team must go first");
                    }
                    break;
                #endregion

                #region GAMESTATE BLUE_TEAM_TURN_START
                case GameState.BLUE_TEAM_TURN_START:
                    currentTurn = GameState.BLUE_TEAM_TURN_START;
                    blueTurnCoroutine = StartCoroutine(EventManager.DelayInvoke(waitTimePerTeamTurn, EventManager.onGameStateChangeDone, GameState.BLUE_TEAM_TURN_TIMEOUT));
                    break;
                #endregion

                #region GAMESTATE RED_TEAM_TURN_START
                case GameState.RED_TEAM_TURN_START:
                    currentTurn = GameState.RED_TEAM_TURN_START;
                    redTurnCoroutine = StartCoroutine(EventManager.DelayInvoke(waitTimePerTeamTurn, EventManager.onGameStateChangeDone, GameState.RED_TEAM_TURN_TIMEOUT));
                    break;
                #endregion

                case GameState.BLUE_TEAM_SUBMISSION:
                    if (currentTurn == GameState.BLUE_TEAM_TURN_START)
                    {
                        StopCoroutine(blueTurnCoroutine);
                        //Todo Score Submission
                        
                    }
                    break;

                case GameState.RED_TEAM_SUBMISSION:
                    if (currentTurn == GameState.RED_TEAM_TURN_START)
                    {
                        StopCoroutine(redTurnCoroutine);
                        //Todo Score Submission
                        
                    }
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
        }

        
    }
}
