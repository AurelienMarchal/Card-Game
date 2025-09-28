using System.Collections.Generic;
using UnityEngine;


namespace GameLogic
{
    using GameAction;
    namespace GameEffect
    {
        public interface AffectsEntitiesInterface
        {
            public bool CheckTriggerToUpdateEntitiesAffected(Action action);

            public void UpdateEntitiesAffected();
            public List<Entity> GetEntitiesAffected();
        }
    }
}
