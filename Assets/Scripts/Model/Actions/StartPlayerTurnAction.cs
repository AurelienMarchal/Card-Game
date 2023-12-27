using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlayerTurnAction : PlayerAction
{
    public StartPlayerTurnAction(Player player, Action requiredAction = null) : base(player, requiredAction)
    {
    
    }

    protected override bool Perform()
    {
        Game.currentGame.PileAction(new PlayerResetMovementAction(player, this));
        Game.currentGame.PileAction(new PlayerIncreaseMaxMouvementAction(player, this));
        return true;
    }
}
