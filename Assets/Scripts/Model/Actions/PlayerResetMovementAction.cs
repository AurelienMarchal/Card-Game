using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResetMovementAction : PlayerAction
{
    public PlayerResetMovementAction(Player player, Action requiredAction = null) : base(player, requiredAction)
    {
    }

    protected override bool Perform()
    {
        player.ResetMovement();
        return true;
    }
}
