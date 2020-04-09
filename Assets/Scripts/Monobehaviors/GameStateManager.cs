using UnityEngine;
using System.Collections.Generic;
using CodeNames;

namespace CodeNames
{
    public class GameStateManager : Object
    {
        Deck deck; //The deck is considered the playing field. Unused cards exist in the SQLite DB
        KeyCard keyCard;
        int CardsToWinTeamRed;
        int CardsToWinTeamBlue;

        private static GameStateManager instance = null;
        public delegate void OnStateChangeHandler();
        public event OnStateChangeHandler OnStateChange;
        public GameStates gameState { get; private set; }

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

        protected GameStateManager()
        {
            LoadKeyCardFromDB();
            LoadDeckFromDB();
            InitializeScore();
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

            Debug.Log("Cards Left to Win...");
            Debug.Log("Red: " + CardsToWinTeamRed.ToString());
            Debug.Log("Blue: " + CardsToWinTeamBlue.ToString());
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

        public void DrawNewDeck()
        {
            LoadDeckFromDB();
        }

        public Deck Deck
        {
            get { return deck; }
        }

        public void SetGameState(GameStates state)
        {
            this.gameState = state;
            OnStateChange();
        }

        public void OnApplicationQuit()
        {
            GameStateManager.instance = null;
        }

    }
}
