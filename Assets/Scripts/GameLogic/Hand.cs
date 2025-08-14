using System.Collections;
using System.Collections.Generic;

namespace GameLogic{
    using System;
    using GameState;
    public class Hand {
        
        private List<Card> cards{
            get;
            set;
        }

        public int handSize{
            get
            {
                return cards.Count;
            }
        }

        public static readonly int maxHandSize = 10;

        
        //Maybe don't need player 
        public Player player
        {
            get;
            private set;
        }

        public Hand(Player player){
            cards = new List<Card>();
            this.player = player;
        }

        [Obsolete]
        public Hand(Player player, List<Card> cards)
        {
            this.player = player;
            this.cards = cards;
        }


        //-1 means at the end 
        public bool TryToAddCard(Card card, int position = -1)
        {
            var canAddCard = CanAddCard(card, position);
            if (canAddCard)
            {
                AddCard(card, position);
            }

            return canAddCard;
        }

        public bool CanAddCard(Card card, int position = -1)
        {
            if (handSize >= maxHandSize)
            {
                return false;
            }

            if (card == null)
            {
                return false;
            }

            var cardIndex = position >= 0 ? position : handSize + position + 1;

            if (cardIndex < 0 || cardIndex > handSize)
            {
                return false;
            }

            return true;
        }

        private void AddCard(Card card, int position = -1)
        {
            var cardIndex = position >= 0 ? position : handSize + position + 1;

            if (cardIndex < 0)
            {
                return;
            }

            else if (cardIndex >= handSize)
            {
                cards.Add(card);
            }

            else
            {
                cards.Insert(cardIndex, card);
            }
        }

        public Card GetCardInPosition(int position)
        {
            var cardIndex = position >= 0 ? position : handSize + position + 1;
            if (cardIndex >= handSize || cardIndex < 0)
            {
                return null;
            }
            else
            {
                return cards[cardIndex];
            }
        }

        

        public HandState ToHandState()
        {
            HandState handState = new HandState();
            handState.cardStates = new List<CardState>();

            foreach (Card card in cards)
            {
                handState.cardStates.Add(card.ToCardState());
            }

            return handState;
        }
    }

}