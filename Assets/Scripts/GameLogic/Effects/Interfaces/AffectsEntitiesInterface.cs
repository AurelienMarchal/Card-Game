using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameLogic
{
    namespace GameEffect
    {
        public interface AffectsEntitiesInterface 
        {   

            public void UpdateEntitiesAffected();
            public List<Entity> GetEntitiesAffected();
        }
    }
}
