using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIncreaseMaxManaAction : PlayerAction
{
    public PlayerIncreaseMaxManaAction(Player player, Action requiredAction = null) : base(player, requiredAction)
    {
    }

    protected override bool Perform()
    {
        return player.TryToIncreaseMaxMana();
    }
}
