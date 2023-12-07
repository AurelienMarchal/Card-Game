public class PlayerUseMovementAction : PlayerAction{

    public int numberOfMovement{
        get;
        protected set;
    }

    public PlayerUseMovementAction(Player player, int numberOfMovement, Action requiredAction = null) : base(player, requiredAction){
        this.numberOfMovement = numberOfMovement;
    }

    protected override bool Perform(){
        return player.TryToUseMovement(numberOfMovement);
    }

}
