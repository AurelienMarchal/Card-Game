using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{
        public class EntityPlayCardAction : EntityAction
        {

            public Card card{
                get;
                private set;
            }


            public EntityPlayCardAction(Entity entity, Card card, Action requiredAction = null) : base(entity, requiredAction){
                this.card = card;
            }



            protected override bool Perform()
            {
                return entity.TryToPlayCard(card);
            }

            public override ActionState ToActionState()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}

