using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPayMouvementCostAction : PlayerAction
{
    
    public int mouvementCost{
        get;
        private set;
    }

    public Dictionary<Entity, int> mouvementCostDistribution{
        get;
        private set;
    }

    public PlayerPayMouvementCostAction(Player player, int mouvementCost, Dictionary<Entity, int> mouvementCostDistribution, Action requiredAction = null) : base(player, requiredAction){
        this.mouvementCostDistribution = mouvementCostDistribution;
        this.mouvementCost = mouvementCost;
    }
}