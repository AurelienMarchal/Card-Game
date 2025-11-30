

namespace GameLogic
{
    using System.Collections.Generic;
    using GameEffect;
    public class BombCard : EntityCard
    {
        public BombCard(Player player) :
            base(
                3,
                player,
                new Cost(mana: 2),
                new Entity(
                    player,
                    EntityModel.Bomb,
                    "Bomb",
                    Tile.noTile,
                    new Health(new HeartType[] { HeartType.Stone }),
                    0,
                    new Damage(0),
                    0,
                    new Cost(mouvement: 1),
                    new Cost(mouvement: 1)
                )
            )
        {
        }


        public override string GetCardName()
        {
            return "Bomb";
        }

        public override string GetText()
        {
            return "Explodes after 3 turns";
        }

        public override List<Effect> GetEffects()
        {
            return new List<Effect>{new EntityExplodeAfterXTurnEffect(3, 5, new Damage(3), entity)};
        }
    }
    

}

