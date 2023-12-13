public class FireballCard : Card
{

    


    public FireballCard(Player player) : base(player, new Cost(new Heart[0], 2), "Fireball", "Ca part comme un pet le machin", false, false)
    {


    }

    protected override bool Activate()
    {
        
        var projectileEffect = new ThrowProjectileEffect(player.hero, player.hero.direction, new Damage(2), 4);
        return projectileEffect.TryToActivate(true);
    }
}