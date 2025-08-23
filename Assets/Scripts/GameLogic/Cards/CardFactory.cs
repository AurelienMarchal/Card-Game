using GameLogic.GameEffect;

namespace GameLogic
{
    public static class CardFactory
    {
        public static Card CreateCardWithNum(uint cardNum)
        {
            //TEMP
            switch (cardNum)
            {
                //case 0: return new Card(cardNum, new ThrowProjectileActivableEffect(null, new Cost(1), new Damage(2), 4));
                //case 1: return new Card(cardNum, new EntityHealsActivableEffect(2, null, new Cost(1)));
                //case 2 : return new Card(cardNum, new EntitySpawnEntityActivableEffect(null, ))
                default: return new Card(cardNum, new Cost(0));
            }
            
        }
    }
}