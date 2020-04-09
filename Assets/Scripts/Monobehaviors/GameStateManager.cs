using UnityEngine;
using System.Collections.Generic;
using CodeNames;

namespace CodeNames
{
    public class GameStateManager : Object
    {
        Deck deck; //The deck is considered the playing field. Unused cards exist in the SQLite DB
        KeyCard keyCard;

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
            Debug.Log("Pulling Cards for the Deck");
            LoadKeyCardFromDB();
            LoadDeckFromDB();
        }

        protected void LoadKeyCardFromDB()
        {
            deck = new Deck();
            keyCard = SqliteApi.GetRandomKeyCard();
        }

        protected void LoadDeckFromDB()
        {
            deck = new Deck();
            List<string> words = SqliteApi.GetRandomWords25();
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
