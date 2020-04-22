using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeNames;

namespace CodeNames
{
    public class Deck
    {
        private List<Card> allCards = new List<Card>(); //Holds a copy of all the cards that belong to this deck
        private List<Card> cards = new List<Card>(); //Holds only the cards in the deck that haven't been dealt

        public List<Card> Cards 
        {
            get { return cards; } 
            set { cards = value; } 
        }

        public int Count
        {
            get { return cards.Count; }
        }

        public Card DealCard()
        {
            Card firstCard = cards[0];
            cards.RemoveAt(0);
            return firstCard;
        }

        public int Add(Card card)
        {
            allCards.Add(card);
            cards.Add(card);
            return Cards.Count;
        }

        public void ResetDeck()
        {
            cards = new List<Card>(allCards);
        }
    }
}