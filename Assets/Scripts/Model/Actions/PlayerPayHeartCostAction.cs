using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPayHeartCostAction : PlayerAction
{
    
    public HeartType[] hearts{
        get;
        private set;
    }

    public PlayerPayHeartCostAction(Player player, HeartType[] hearts, Action requiredAction = null) : base(player, requiredAction){
        this.hearts = hearts;
    }


    protected override bool Perform(){
        return player.TryToPayHeartCost(hearts);
    }
}
