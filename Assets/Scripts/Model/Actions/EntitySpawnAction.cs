using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawnAction : EntityAction
{
    public EntitySpawnAction(Entity entity, Tile spawnTile,  Action requiredAction = null) : base(entity, requiredAction){
        
    }
}