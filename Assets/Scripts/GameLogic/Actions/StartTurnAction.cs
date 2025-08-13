using System.Collections;
using System.Collections.Generic;
using GameLogic.GameState;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{
        public class StartTurnAction : Action
        {

            public StartTurnAction(Action requiredAction = null) : base(requiredAction)
            {

            }
            
            protected override bool Perform()
            {
                Game.currentGame.StartTurn();
                Game.currentGame.PileAction(new PlayerStartTurnAction(Game.currentGame.currentPlayer, this));
                return true;
            }

            public override ActionState ToActionState()
            {
                var actionState = new StartTurnActionState();
                actionState.newTurnCount = Game.currentGame.turn;
                return actionState;
            }
        }
    }
}
