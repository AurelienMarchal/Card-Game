using System;
using System.Collections.Generic;

namespace GameLogic
{
    public class Deck
    {
        public Player player
        {
            get;
            private set;
        }

        public int deckSize
        {
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

        public Deck(uint[] deckList, Player player, System.Random random)
        {
            this.player = player;
            cards = new List<Card>();
            this.random = random;

            foreach (var cardNum in deckList)
            {
                cards.Add(CardFactory.CreateCardWithNum(cardNum, player));
            }
        }

        public Card TryToDraw()
        {
            var canDraw = CanDraw();
            if (canDraw)
            {
                return Draw();
            }
            return null;
        }

        public bool CanDraw()
        {
            if (cards == null)
            {
                return false;
            }

            if (cards.Count == 0)
            {
                return false;
            }


            return true;
        }

        private Card Draw()
        {
            if (cards == null)
            {
                return null;
            }

            if (cards.Count == 0)
            {
                return null;
            }

            Card card = cards[cards.Count - 1];

            cards.RemoveAt(cards.Count - 1);

            return card;
        }


        public void Shuffle()
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }
    }
}