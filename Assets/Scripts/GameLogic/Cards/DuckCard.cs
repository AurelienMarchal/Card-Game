

namespace GameLogic
{
    using GameEffect;
    public class DuckCard : EntityCard
    {
        public DuckCard(Player player) :
            base(
                2,
                player,
                new Cost(mana: 1),
                new Entity(
                    player,
                    EntityModel.Duck,
                    "Duck",
                    Tile.noTile,
                    new Health(new HeartType[] { HeartType.Red }),
                    3,
                    new Damage(1),
                    1,
                    new Cost(mouvement: 1),
                    new Cost(mouvement: 1)
                )
            )
        {
            entity.AddEffect(new EntityGivesAtkBuffWhenNextToEntitiesEffect(2, entity));
        }


        public override string GetCardName()
        {
            return "Canard";
        }

        public override string GetText()
        {
            return "Coin coin";
        }
    }
    

}

