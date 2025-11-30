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
                case 1 : return new HealTargetEntityCard(player);
                case 2 : return new DuckCard(player);
                
                
                default: return new Card(cardNum, player, new Cost(mana:1));
            }
            
        }
    }
}