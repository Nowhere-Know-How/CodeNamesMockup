using UnityEngine;
using System.Collections.Generic;
using CodeNames;

namespace CodeNames
{
    public class GameStateManager
    {
        static Deck deck; //The deck is considered the playing field. Unused cards exist in the SQLite DB
        static KeyCard keyCard;
        static int CardsToWinTeamRed;
        static int CardsToWinTeamBlue;

        static Team redTeam = new Team();
        static Team blueTeam = new Team();
        static CardColor teamWithFirstTurn;
        static int minPlayersRequired = 4;

        static bool isGameDataInited = false;
        static bool isEventListenersInited = false;
        static bool isTeamsPicked = false;
        
        public static bool IsGameDataInited
        {
            get { return isGameDataInited; }
        }
        public static bool IsTeamsPicked
        {
            get { return isTeamsPicked; }
        }
        public static CardColor TeamWithFirstTurn
        {
            get { return teamWithFirstTurn; }
        }


        public static void InitEventListeners()
        {
            if (!isEventListenersInited)
            {
                Debug.Log("Event Listeners Registered");
                EventManager.onGameStateChange.AddListener(HandleStateChange);
                isEventListenersInited = true;
            }
        }

        private GameStateManager()
        {
        }

        private static void HandleStateChange(GameState gs)
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
                    break;
                #endregion

                #region GAMESTATE WAIT
                case GameState.WAIT_FOR_TEAMS_TO_MEET_EACH_OTHER:
                    Debug.Log("Game State Manager is waiting...");
                    break;
                #endregion

                default:
                    throw new System.NotImplementedException("GameState Not Implemented: " + gs.ToString());
            }
        }

        protected static void InitializeScore()
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

        public static RevealCardResolutions RevealCard(int index) //Reveals the card at the index of the deck
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

        public static KeyCard KeyCard
        {
            get { return keyCard; }
        }
        public static void DrawNewDeck()
        {
            LoadDeckFromDB();
        }

        public static Deck Deck
        {
            get { return deck; }
        }
        protected static void LoadKeyCardFromDB()
        {
            Debug.Log("Drawing KeyCard");
            deck = new Deck();
            keyCard = SqliteApi.GetRandomKeyCard();
        }

        protected static void LoadDeckFromDB()
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
