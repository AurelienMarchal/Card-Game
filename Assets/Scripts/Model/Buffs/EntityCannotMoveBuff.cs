


namespace GameLogic{

    namespace GameBuff{
        public class EntityCannotMoveBuff : EntityBuff{
            public EntityCannotMoveBuff() : base("Cannot move")
            {
            }


            public override int IsPositive()
            {
                return -1;
            }
        }
    }
}