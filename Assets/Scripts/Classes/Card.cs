using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeNames { 
    public class Card
    {
        private string text = null;
        public Card(string word)
        {
            text = word;
        }

        public string Text   
        {
            get { return text; }
            set { text = value; }
        }
    }
}