using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeNames
{
    public class Clue
    {
        CardColor team;
        string word;
        int number;

        public CardColor Team
        {
            get { return team; }
            set { team = value; }
        }
        public string Word
        {
            get { return word; }
            set { word = value; }
        }

        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        public Clue(CardColor color, string w, int clue_count)
        {
            team = color;
            word = w;
            number = clue_count;
        }

        public override string ToString()
        {
            return team.ToString() + ": " + word + ", " + number.ToString();
        }
    }
}
