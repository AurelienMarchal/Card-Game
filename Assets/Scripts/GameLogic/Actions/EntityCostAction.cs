
using GameLogic.GameState;

namespace GameLogic{

    namespace GameAction{
        public abstract class EntityCostAction : EntityAction{
            public EntityCostAction(Entity entity, Action requiredAction = null) : base(entity, requiredAction)
            {
            
            
            }
        }
    }
}