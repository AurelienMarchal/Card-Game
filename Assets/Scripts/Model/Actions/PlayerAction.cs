
public class PlayerAction : Action
{
    public Player player{
        get;
        protected set;
    }
    public PlayerAction(Player player, Action requiredAction = null) : base(requiredAction){
        this.player = player;
    }
}
