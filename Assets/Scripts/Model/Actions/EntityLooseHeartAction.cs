using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityLooseHeartAction : EntityAction
{
    public Heart heart{
        get;
        private set;
    }

    public EntityLooseHeartAction(Entity entity, Heart heart, Action requiredAction = null) : base(entity, requiredAction)
    {  
        this.heart = heart;
    }
}
