namespace GameLogic
{
    namespace GameState
    {
        public class PlayerPlayCardActionState : PlayerActionState
        {
            public CardState card{
                get;
                set;
            }
            
            //None = -1
            public int targetTileNum
            {
                get;
                set;
            }

            //None = -1
            public int targetEntityPlayerNum
            {
                get;
                set;
            }

            //None = -1
            public int targetEntityNum
            {
                get;
                set;
            }
        }
    }
}