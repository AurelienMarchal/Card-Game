public class ThrowProjectileSpellCard : Card
{

    public Damage damage{
        get;
        protected set;
    }

    public int maxRange{
        get;
        protected set;
    }

    public ThrowProjectileSpellCard(Player player, Cost cost, string cardName, string text, Damage damage, int maxRange, bool needsTileTarget = false, bool needsEntityTarget = false) : base(player, cost, cardName, text, needsTileTarget, needsEntityTarget){
        this.damage = damage;
        this.maxRange = maxRange;
    }

    public ThrowProjectileSpellCard(Player player, ScriptableThrowProjectileCard scriptableThrowProjectileCard) : base(player, scriptableThrowProjectileCard){
        damage = scriptableThrowProjectileCard.damage;
        maxRange = scriptableThrowProjectileCard.maxRange;
    }

    protected override bool Activate(){
        var projectileEffect = new ThrowProjectileEffect(player.hero, player.hero.direction, damage, maxRange);
        return projectileEffect.TryToActivate(true);
    }
}