

namespace GameLogic{

    using GameBuff;

    namespace GameEffect{
        public class CannotMoveUntilEndOfPlayerTurnEffect : UntilEndOfPlayerTurnEntityEffect{
            public CannotMoveUntilEndOfPlayerTurnEffect(Entity entity, bool displayOnUI = true) : base(entity, displayOnUI)
            {
                entityBuffs.Add(new EntityCannotMoveBuff());
            }

            public override string GetEffectText()
            {
                return $"{associatedEntity} cannot move until the end of the turn";
            }

            
        }
    }
}
