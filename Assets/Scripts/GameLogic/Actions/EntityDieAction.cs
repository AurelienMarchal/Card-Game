using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{
        public class EntityDieAction : EntityAction
        {
            public EntityDieAction(Entity entity, Action requiredAction = null) : base(entity, requiredAction)
            {

            }

            protected override bool Perform()
            {
                Game.currentGame.board.entities.Remove(entity);
                return true;
            }
            
            public override ActionState ToActionState()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
