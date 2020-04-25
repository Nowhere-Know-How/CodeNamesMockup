using UnityEngine;
using System.Collections.Generic;
using CodeNames;
using System.Collections;
using System;

namespace CodeNames
{
    public class GameStateApi : SingletonBehaviour<GameStateApi>
    {
        public int guessesLeft = -1;

        int minPlayersRequired = 4;

        Deck deck; //The deck is considered the playing field. Unused cards exist in the SQLite DB
        KeyCard keyCard;
        
        int CardsToWinTeamRed;
        Team redTeam = new Team();

        int CardsToWinTeamBlue;
        Team blueTeam = new Team();

        CardColor teamWithFirstTurn;

        Clue lastClue;

        bool isEventListenersInited = false;
        
        public CardColor TeamWithFirstTurn
        {
            get { return teamWithFirstTurn; }
        }
        void Awake()
        {
            InitEventListeners();
            Debug.Log("ENV");
            Debug.Log(Environment.GetEnvironmentVariable("PRODUCTION"));
        }

        public void InitEventListeners()
        {
            if (!isEventListenersInited)
            {
                Debug.Log("GameStateManager Event Listeners Registered");
                EventManager.onGameStateChange.AddListener(HandleStateChange);
                EventManager.onForwardedTeamSubmission.AddListener(HandleTeamSubmission);
                EventManager.onForwardedCodeMasterSubmission.AddListener(HandleCodeMasterSubmission);
                isEventListenersInited = true;
            }
        }

        private GameStateApi()
        {
        }

        private void HandleCodeMasterSubmission(Clue clue)
        {
            lastClue = clue;
            if (clue.Number <= 0)
            {
                guessesLeft = 25;
            }
            else
            {
                guessesLeft = clue.Number + 1;
            }

            switch (clue.Team)
            {
                case CardColor.Blue:
                    EventManager.onGameStateControllerChange.Invoke(GameState.BLUE_TEAM_TURN_CODEMASTER_SUBMISSION_DONE);
                    break;

                case CardColor.Red:
                    EventManager.onGameStateControllerChange.Invoke(GameState.RED_TEAM_TURN_CODEMASTER_SUBMISSION_DONE);
                    break;

                default:
                    throw new NotImplementedException("Clue team not implemented: " + clue.Team.ToString());
            }

        }

        private void HandleTeamSubmission(TeamCardSubmission submission)
        {
            //Debug.Log("Team Submission received by Game Manager: " + submission.ToString());
            RevealCardResolutions revealResolution = RevealCard(submission.CardIndex);
            HandleRevealCardResolution(revealResolution, submission.TeamColor);
        }

        public void HandleRevealCardResolution(RevealCardResolutions revealResolution, CardColor teamColor)
        {
            switch (revealResolution)
            {
                case RevealCardResolutions.PASS:
                    Debug.Log("Team passed turn");
                    if (teamColor == CardColor.Blue)
                    {
                        EventManager.onGameStateApiDone.Invoke(GameState.BLUE_TEAM_TURN_END);
                    }
                    else if (teamColor == CardColor.Red)
                    {
                        EventManager.onGameStateApiDone.Invoke(GameState.RED_TEAM_TURN_END);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("A team must be blue or red");
                    }
                    break;

                case RevealCardResolutions.ALREADY_REVEALED:
                    Debug.Log("Ignored Submission. Already revealed card was chosen");
                    break;

                case RevealCardResolutions.REVEALED_BLACK_TEAM_CARD:
                    Debug.Log("Black card revealed! " + teamColor.ToString() + " team loses the game");
                    if (teamColor == CardColor.Blue)
                    {
                        EventManager.onGameStateApiDone.Invoke(GameState.RED_TEAM_WINS);
                    }
                    else if (teamColor == CardColor.Red)
                    {
                        EventManager.onGameStateApiDone.Invoke(GameState.BLUE_TEAM_WINS);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("A team must be blue or red");
                    }
                    break;

                case RevealCardResolutions.REVEALED_BROWN_TEAM_CARD:
                    Debug.Log("Brown card revealed! " + teamColor.ToString() + " team loses their turn");
                    if (teamColor == CardColor.Blue)
                    {
                        EventManager.onGameStateApiDone.Invoke(GameState.BLUE_TEAM_TURN_END);
                    }
                    else if (teamColor == CardColor.Red)
                    {
                        EventManager.onGameStateApiDone.Invoke(GameState.RED_TEAM_TURN_END);
                    }
                    else
                    {
                        throw new System.InvalidOperationException("A team must be blue or red");
                    }
                    break;

                case RevealCardResolutions.REVEALED_BLUE_TEAM_CARD:
                    guessesLeft -= 1;
                    CardsToWinTeamBlue -= 1;
                    PrintScore();
                    if (CardsToWinTeamBlue == 0)
                    {
                        EventManager.onGameStateApiDone.Invoke(GameState.BLUE_TEAM_WINS);
                    }
                    else
                    {
                        if (teamColor == CardColor.Blue)
                        {
                            if (guessesLeft == 0)
                            {
                                EventManager.onGameStateApiDone.Invoke(GameState.BLUE_TEAM_TURN_NO_MORE_GUESSES);
                            }
                            else
                            {
                                Debug.Log("Blue card revealed! Team continues. Guesses left: " + guessesLeft.ToString());
                                EventManager.onGameStateApiDone.Invoke(GameState.BLUE_TEAM_TURN_CONTINUE);
                            }
                        }
                        else if (teamColor == CardColor.Red)
                        {
                            Debug.Log("Blue card revealed! " + teamColor.ToString() + " team loses their turn");
                            EventManager.onGameStateApiDone.Invoke(GameState.RED_TEAM_TURN_END);
                        }
                        else
                        {
                            throw new System.InvalidOperationException("A team must be blue or red");
                        }
                    }
                    break;



                case RevealCardResolutions.REVEALED_RED_TEAM_CARD:
                    guessesLeft -= 1;
                    CardsToWinTeamRed -= 1;
                    PrintScore();
                    if (CardsToWinTeamRed == 0)
                    {
                        EventManager.onGameStateApiDone.Invoke(GameState.RED_TEAM_WINS);
                    }
                    else
                    {
                        if (teamColor == CardColor.Blue)
                        {
                            Debug.Log("Red card revealed! " + teamColor.ToString() + " team loses their turn");
                            EventManager.onGameStateApiDone.Invoke(GameState.BLUE_TEAM_TURN_END);
                        }
                        else if (teamColor == CardColor.Red)
                        {
                            if (guessesLeft == 0)
                            {
                                EventManager.onGameStateApiDone.Invoke(GameState.RED_TEAM_TURN_NO_MORE_GUESSES);
                            }
                            else
                            {
                                Debug.Log("Red card revealed! Team continues. Guesses left: " + guessesLeft.ToString());
                                EventManager.onGameStateApiDone.Invoke(GameState.RED_TEAM_TURN_CONTINUE);
                            }
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



        private void HandleStateChange(GameState gs)
        {
            //Debug.Log("GAME STATE CHANGED: " + gs.ToString());
            switch (gs)
            {
                #region GAMESTATE INIT
                case GameState.INIT:
                    LoadKeyCardFromDB();
                    LoadDeckFromDB();
                    InitializeScore();
                    EventManager.onGameStateApiDone.Invoke(GameState.INIT_DONE);
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
                        EventManager.onGameStateApiDone.Invoke(GameState.PICK_TEAMS_DONE_BLUE_TO_START);
                    }
                    else if (teamWithFirstTurn == CardColor.Red)
                    {
                        EventManager.onGameStateApiDone.Invoke(GameState.PICK_TEAMS_DONE_RED_TO_START);
                    }
                    else
                    {
                        throw new System.NotSupportedException("A red or blue team must go first");
                    }
                    break;
                #endregion

                default:
                    throw new System.NotImplementedException("GameState Not Implemented: " + gs.ToString());
            }
        }


        private void InitializeScore()
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

        private RevealCardResolutions RevealCard(CardChoice cardChoice) //Reveals the card at the index of the deck
        {
            try
            {
                if (cardChoice == CardChoice.PASS)
                {
                    return RevealCardResolutions.PASS;
                }

                int index = (int)cardChoice;
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

        private KeyCard KeyCard
        {
            get { return keyCard; }
        }
        private void DrawNewDeck()
        {
            LoadDeckFromDB();
        }

        private Deck Deck
        {
            get { return deck; }
        }
        private void LoadKeyCardFromDB()
        {
            Debug.Log("Drawing KeyCard");
            deck = new Deck();
            keyCard = SqliteApi.GetRandomKeyCard();
        }

        private void LoadDeckFromDB()
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

        private void PrintScore()
        {
            Debug.Log("Cards Left to Win: " + "\nRed - " + CardsToWinTeamRed.ToString() + ", Blue - " + CardsToWinTeamBlue.ToString());
        }
        
    }
}
