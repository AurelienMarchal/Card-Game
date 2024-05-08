using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityChangeDirectionAction : EntityAction
{
    
    public Direction newDirection{
        get;
        protected set;
    }

    public EntityChangeDirectionAction(Entity entity, Direction newDirection, Action requiredAction = null) : base(entity, requiredAction)
    {
        this.newDirection = newDirection;
    }

    protected override bool Perform(){
        return entity.TryToChangeDirection(newDirection);
    }
}
