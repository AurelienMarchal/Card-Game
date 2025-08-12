using GameLogic.GameEffect;

namespace GameLogic
{
    public static class CardFactory
    {
        public static Card CreateCardWithNum(uint cardNum)
        {
            return new Card(new ActivableEffect(null, new Cost(1)));
        }
    }
}