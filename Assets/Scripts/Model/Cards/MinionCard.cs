public class MinionCard : Card
{

    public Health minionHealth{
        get;
        protected set;
    }

    public MinionCard(Player player, Cost cost, Health minionHealth, string cardName, string text, bool needsTileTarget = false, bool needsEntityTarget = false) : base(player, cost, cardName, text, needsTileTarget, needsEntityTarget){
        this.minionHealth = minionHealth;
    }


    public MinionCard(Player player, ScriptableMinionCard scriptableMinionCard) : base(player, scriptableMinionCard){
        this.minionHealth = scriptableMinionCard.minionHealth;
    }
}