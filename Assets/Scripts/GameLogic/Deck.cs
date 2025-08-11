using System;
using System.Collections.Generic;

namespace GameLogic
{
    public class Deck
    {
        public int cardCount{
            get
            {
                return cards.Count;
            }
        }

        private List<Card> cards
        {
            get; set;
        }

        private Random random;

        public Deck(uint[] deckList, System.Random random)
        {
            cards = new List<Card>();
            this.random = random;

            foreach (var cardNum in deckList)
            {
                cards.Add(CardFactory.CreateCardWithNum(cardNum));
            }
        }


        public void Shuffle()
        {
            int n = cards.Count;
            while (n > 1) {  
                n--;  
                int k = random.Next(n + 1);  
                Card value = cards[k];  
                cards[k] = cards[n];  
                cards[n] = value;  
            }  
        }
    }
}