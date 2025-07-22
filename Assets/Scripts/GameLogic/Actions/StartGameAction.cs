using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{
        public class StartGameAction : Action
        {
            public StartGameAction(Action requiredAction = null) : base(requiredAction)
            {

            }

            protected override bool Perform()
            {
                Game.currentGame.StartGame();
                Game.currentGame.PileAction(new StartTurnAction(this));

                return true;
            }
            
            public override ActionState ToActionState()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
