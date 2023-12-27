using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlayerTurnAction : PlayerAction
{
    public EndPlayerTurnAction(Player player, Action requiredAction = null) : base(player, requiredAction)
    {
    }

    protected override bool Perform()
    {
        var changeTurn = Game.currentGame.EndPlayerTurn();
        if(changeTurn){
            Game.currentGame.PileAction(new StartTurnAction(this));
        }
        else{
            Game.currentGame.PileAction(new StartPlayerTurnAction(Game.currentGame.currentPlayer, this));
        }
        
        return true;
    }
}
