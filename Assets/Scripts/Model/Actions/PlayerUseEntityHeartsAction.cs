
public class PlayerUseEntityHeartsAction : PlayerAction
{
    public Entity entity{ 
        get; 
        private set; 
    }

    public HeartType[] hearts{
        get;
        private set;
    }
    public PlayerUseEntityHeartsAction(Player player, Entity entity, HeartType[] hearts, Action requiredAction = null) : base(player, requiredAction)
    {
        this.entity = entity;
        this.hearts = hearts;
    }
}