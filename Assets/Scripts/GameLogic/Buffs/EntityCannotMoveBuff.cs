


namespace GameLogic{

    namespace GameBuff{
        public class EntityCannotMoveBuff : Buff{
            public EntityCannotMoveBuff(string assiociatedEffectId) : base("Cannot move", assiociatedEffectId)
            {
            }


            public override int IsPositive()
            {
                return -1;
            }
        }
    }
}