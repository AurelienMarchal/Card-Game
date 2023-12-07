using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPayHeartCostAction : PlayerAction
{
    
    public Heart[] hearts{
        get;
        private set;
    }

    public PlayerPayHeartCostAction(Player player, Heart[] hearts, Action requiredAction = null) : base(player, requiredAction){
        this.hearts = hearts;
    }


    protected override bool Perform(){
        return player.TryToPayHeartCost(hearts);
    }
}
