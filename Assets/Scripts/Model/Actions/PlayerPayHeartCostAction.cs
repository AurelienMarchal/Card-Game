using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPayHeartCostAction : PlayerAction
{
    
    public HeartType[] hearts{
        get;
        private set;
    }

    public Dictionary<Entity, HeartType[]> heartCostDistribution{
        get;
        private set;
    }

    public PlayerPayHeartCostAction(Player player, HeartType[] hearts, Dictionary<Entity, HeartType[]> heartCostDistribution, Action requiredAction = null) : base(player, requiredAction){
        this.hearts = hearts;
        this.heartCostDistribution = heartCostDistribution;
    }
}
