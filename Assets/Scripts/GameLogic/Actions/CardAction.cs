using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic{

            namespace GameAction{
        public class CardAction : Action{
            
            public Card card{
                get;
                protected set;
            }

            public CardAction(Card card, Action requiredAction): base(requiredAction){
                this.card = card;
            }
        }
    }
}