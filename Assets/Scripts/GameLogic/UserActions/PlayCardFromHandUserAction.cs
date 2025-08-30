namespace GameLogic{

    namespace UserAction{
        public class PlayCardFromHandUserAction : UserAction
        {   
            public int cardPositionInHand
            {
                get;
                private set;
            }
            
            //None = -1
            public int entityTargetPlayerNum
            {
                get;
                private set;
            }

            //Temp, certain card might need multiple targets
            //None = -1
            public int entityTargetNum
            {
                get;
                private set;
            }

            //Temp, certain card might need multiple targets
            //None = -1
            public int tileTargetNum
            {
                get;
                private set;
            }


            public PlayCardFromHandUserAction(
                uint playerNum,
                int cardPositionInHand,
                int entityTargetPlayerNum = -1,
                int entityTargetNum = -1,
                int tileTargetNum = -1) : base(playerNum)
            {
                this.cardPositionInHand = cardPositionInHand;
                this.entityTargetPlayerNum = entityTargetPlayerNum;
                this.entityTargetNum = entityTargetNum;
                this.tileTargetNum = tileTargetNum;

            }
        }
    }
}