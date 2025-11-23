namespace GameLogic{

    namespace UserAction{
        public class ActivateEntityEffectUserAction : EntityUserAction
        {

            public string effectId
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

            public ActivateEntityEffectUserAction(
                uint playerNum,
                uint entityNum,
                string effectId,
                int entityTargetPlayerNum = -1,
                int entityTargetNum = -1,
                int tileTargetNum = -1

                ) : base(playerNum, entityNum)
            {
                this.effectId = effectId;
                this.entityTargetPlayerNum = entityTargetPlayerNum;
                this.entityTargetNum = entityTargetNum;
                this.tileTargetNum = tileTargetNum;
            }
        }
    }
}