public class PlayerUseManaAction : PlayerAction{

    public int numberOfMana{
        get;
        protected set;
    }

    public PlayerUseManaAction(Player player, int numberOfMana, Action requiredAction = null) : base(player, requiredAction){
        this.numberOfMana = numberOfMana;
    }

    protected override bool Perform(){
        return player.TryToUseMana(numberOfMana);
    }

}
