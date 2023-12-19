using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIncreaseMaxMouvementAction : PlayerAction
{
    public PlayerIncreaseMaxMouvementAction(Player player, Action requiredAction = null) : base(player, requiredAction)
    {
    }

    protected override bool Perform()
    {
        return player.TryToIncreaseMaxMovement();
    }
}
