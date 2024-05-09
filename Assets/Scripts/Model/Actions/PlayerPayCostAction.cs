

using System.Collections.Generic;

public class PlayerPayCostAction : PlayerAction
{
    public Cost cost{
        get;
        private set;
    }

    public Dictionary<Entity, HeartType[]> heartCostDistribution{
        get;
        private set;
    }

    public Dictionary<Entity, int> mouvementCostDistribution{
        get;
        private set;
    }

    public PlayerPayCostAction(
            Player player, 
            Cost cost, Dictionary<Entity, int> mouvementCostDistribution,
            Dictionary<Entity, HeartType[]> heartCostDistribution, 
            Action requiredAction = null) : base(player, requiredAction
        ){
        this.cost = cost;
        this.mouvementCostDistribution = mouvementCostDistribution;
        this.heartCostDistribution = heartCostDistribution;
    }
}