using UnityEngine;
using System.Collections.Generic;
using CodeNames;

namespace CodeNames
{
    // Listens for GameStates Events
    public class GameStateManager : Object
    {
        Deck deck; //The deck is considered the playing field. Unused cards exist in the SQLite DB
        KeyCard keyCard;
        int CardsToWinTeamRed;
        int CardsToWinTeamBlue;

        private static GameStateManager instance = null;

        public static GameStateManager Instance
        {
            get
            {
                if (GameStateManager.instance == null)
                {
                    GameStateManager.instance = new GameStateManager();
                }
                return GameStateManager.instance;
            }
        }
        
        private void Init(GameState gameState){
            Debug.Log("GameState: " + gameState.ToString());
            LoadKeyCardFromDB();
            LoadDeckFromDB();
            InitializeScore();
        }
        
        protected GameStateManager()
        {
            EventManager.onGameStateChange.AddListener(Init);

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
