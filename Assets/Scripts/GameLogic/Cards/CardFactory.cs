using GameLogic.GameEffect;

namespace GameLogic
{
    public static class CardFactory
    {
        public static Card CreateCardWithNum(uint cardNum, Player player)
        {
            //TEMP
            switch (cardNum)
            {
                //case 0: return new Card(cardNum, new ThrowProjectileEntityEffect(null, new Cost(1), new Damage(2), 4));
                //case 1: return new Card(cardNum, new EntityHealsEntityEffect(2, null, new Cost(1)));
                case 2 : return new EntityCard(
                    cardNum,
                    player,
                    new Cost(mana: 1),
                    new Entity(
                        player,
                        EntityModel.Duck,
                        "Duck",
                        Tile.noTile,
                        new Health(new HeartType[] { HeartType.Red }),
                        3,
                        new System.Collections.Generic.List<EntityEffect>()
                    )
                );
                default: return new Card(cardNum, player, new Cost(mana:1));
            }
            
        }
    }
}