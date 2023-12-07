

public class PlayerPayCostAction : PlayerAction
{
    public Cost cost{
        get;
        private set;
    }

    public PlayerPayCostAction(Player player, Cost cost, Action requiredAction = null) : base(player, requiredAction)
    {
        this.cost = cost;
    }

    
}