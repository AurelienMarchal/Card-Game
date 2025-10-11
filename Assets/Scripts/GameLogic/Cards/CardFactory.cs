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
                case 2 : return new DuckCard(player);
                
                default: return new Card(cardNum, player, new Cost(mana:1));
            }
            
        }
    }
}