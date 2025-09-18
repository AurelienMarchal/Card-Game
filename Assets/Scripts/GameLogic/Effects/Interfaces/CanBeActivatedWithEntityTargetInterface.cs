
using System.Collections.Generic;


namespace GameLogic
{
    namespace GameEffect
    {
        public interface CanBeActivatedWithEntityTargetInterface
        {
            public List<Entity> PossibleEntityTargets();

            public bool CanBeActivatedWithEntityTarget(Entity entity);

            protected void ActivateWithEntityTarget(Entity entity);

            public bool TryToActivateWithEntityTarget(Entity entity){
                var result = CanBeActivatedWithEntityTarget(entity);
                if(result){
                    ActivateWithEntityTarget(entity);
                }
                return result;
            }

        }
    }
}
