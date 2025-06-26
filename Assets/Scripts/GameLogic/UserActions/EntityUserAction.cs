namespace GameLogic{

    namespace UserAction{
        public class EntityUserAction : UserAction
        {
            public uint entityNum{ get; private set; }

            public EntityUserAction(uint playerNum, uint entityNum) : base(playerNum)
            {
                this.entityNum = entityNum;
            }

            public EntityUserAction(uint playerNum) : base(playerNum)
            {
            }
        }
    }
}