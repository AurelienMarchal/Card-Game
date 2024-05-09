

public class PlayerUseEntityMouvementAction : PlayerAction
{

    public Entity entity{ 
        get; 
        private set; 
    }

    public int mouvement{ 
        get; 
        private set; 
    }

    public PlayerUseEntityMouvementAction(Player player, Entity entity, int mouvement, Action requiredAction = null) : base(player, requiredAction)
    {
        this.entity = entity;
        this.mouvement = mouvement;
    }
}