
namespace GameLogic{

    namespace GameAction{
        public abstract class EntityAction : Action{
            
            public Entity entity{
                get;
                protected set;
            }
            public EntityAction(Entity entity, Action requiredAction = null) : base(requiredAction){
                this.entity = entity;
            }
        }
    }
}