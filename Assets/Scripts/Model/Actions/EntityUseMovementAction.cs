using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityUseMovementAction : EntityAction
{
    public int movementUsed{
        get;
        private set;
    }

    public EntityUseMovementAction(int movement, Entity entity, Action requiredAction = null) : base(entity, requiredAction){
        movementUsed = movement;
    }

    protected override bool Perform()
    {
        return entity.TryToUseMovement(movementUsed);
    }
}
