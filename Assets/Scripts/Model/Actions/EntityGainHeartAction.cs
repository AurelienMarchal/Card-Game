using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGainHeartAction : EntityAction
{
    public HeartType heartType{
        get;
        private set;
    }

    public EntityGainHeartAction(Entity entity, HeartType heartType, Action requiredAction = null) : base(entity, requiredAction)
    {  
        this.heartType = heartType;
    }

    protected override bool Perform()
    {
        return entity.health.TryToGainHeart(heartType);
    }
}