using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTurnAction : Action
{

    public StartTurnAction(Action requiredAction = null): base(requiredAction){

    }

    protected override bool Perform()
    {
        Game.currentGame.StartTurn();
        Game.currentGame.PileAction(new StartPlayerTurnAction(Game.currentGame.currentPlayer, this));
        return true;
    }
}
