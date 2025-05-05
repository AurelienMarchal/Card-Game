using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic{

    namespace GameAction{
        public class StartGameAction : Action{
            public StartGameAction(Action requiredAction = null) : base(requiredAction){

            }

            protected override bool Perform()
            {
                Game.currentGame.StartGame();
                Game.currentGame.PileAction(new StartTurnAction(this));

                return true;
            }
        }
    }
}
