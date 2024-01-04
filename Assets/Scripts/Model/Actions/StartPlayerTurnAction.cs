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
        foreach (var entity in player.entities){
            Game.currentGame.PileAction(new EntityResetMovementAction(entity, this));
        }
        
        Game.currentGame.PileAction(new PlayerResetManaAction(player, this));
        Game.currentGame.PileAction(new PlayerIncreaseMaxManaAction(player, this));
        return true;
    }
}
