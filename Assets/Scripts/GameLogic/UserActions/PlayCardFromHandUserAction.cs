namespace GameLogic{

    namespace UserAction{
        public class PlayCardFromHandUserAction : UserAction
        {   
            public int cardPositionInHand
            {
                get;
                private set;
            }

            public uint entityTargetPlayerNum
            {
                get;
                private set;
            }

            public uint entityTargetNum
            {
                get;
                private set;
            }

            public uint tileTargetNum
            {
                get;
                private set;
            }


            public PlayCardFromHandUserAction(
                uint playerNum,
                int cardPositionInHand,
                uint entityTargetPlayerNum,
                uint entityTargetNum,
                uint tileTargetNum) : base(playerNum)
            {
                this.cardPositionInHand = cardPositionInHand;
                this.entityTargetPlayerNum = entityTargetPlayerNum;
                this.entityTargetNum = entityTargetNum;
                this.tileTargetNum = tileTargetNum;

            }
        }
    }
}