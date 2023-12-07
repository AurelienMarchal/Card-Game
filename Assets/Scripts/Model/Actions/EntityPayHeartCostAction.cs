using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPayHeartCostAction : EntityAction
{

    public Heart heart{
        get;
        private set;
    }

    public EntityPayHeartCostAction(Entity entity, Heart heart, Action requiredAction = null) : base(entity, requiredAction){
        this.heart = heart;
    }
}
