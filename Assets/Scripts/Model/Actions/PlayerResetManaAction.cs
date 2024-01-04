using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResetManaAction : PlayerAction
{
    public PlayerResetManaAction(Player player, Action requiredAction = null) : base(player, requiredAction)
    {
    }

    protected override bool Perform()
    {
        player.ResetMana();
        return true;
    }
}
