using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTakeDamageAction : EntityAction
{   

    //Damage type
    public EntityTakeDamageAction(Entity entity, int damage,  Action requiredAction = null) : base(entity, requiredAction)
    {
    }
}
