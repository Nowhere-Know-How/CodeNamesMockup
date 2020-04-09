using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeNames
{
    [System.Serializable]
    public class KeyCard
    {
        public int id;
        public int firstToMove;
        public List<CardColor> data = new List<CardColor>();
        public int size;

        public KeyCard() { }

        public KeyCard(int id, int firstToMove, string data_string, int size)
        {
            id = id;
            firstToMove = firstToMove;
            size = size;

            string[] d = data_string.Split(',');
            for (int i = 0; i < d.Length; i++)
            {
                int n = Int32.Parse(d[i].Trim());
                CardColor color = (CardColor)Enum.ToObject(typeof(CardColor), n);
                data.Add(color);
            }
        }
    }
}