using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeNames { 
    public class Card
    {
        private CardState state;
        private string text = null;
        public Card(string word)
        {
            text = word;
        }

        public CardState State
        {
            get { return state; }
            set { state = value; }
        }

        public string Text   
        {
            get { return text; }
            set { text = value; }
        }
    }
}