using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDeathAction : EntityAction
{
    public EntityDeathAction(Entity entity, Action requiredAction = null) : base(entity, requiredAction){
        
    }
}
