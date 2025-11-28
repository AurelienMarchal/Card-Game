using System.Collections.Generic;
using System;


namespace GameLogic
{
    
    using GameAction;
    namespace GameEffect
    {
        public interface AffectsEntitiesInterface
        {
            public bool CheckTriggerToUpdateEntitiesAffected(Action action);

            public Type[] ActionTypeTriggersToUpdateEntitiesAffected();

            public void UpdateEntitiesAffected();
            public List<Entity> GetEntitiesAffected();
        }
    }
}
