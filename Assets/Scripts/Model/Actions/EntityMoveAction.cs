using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMoveAction : EntityAction
{
    
    public Tile startTile;

    public Tile endTile;

    public EntityMoveAction(Entity entity, Tile startTile, Tile endTile) : base(entity)
    {
        this.startTile = startTile;
        this.endTile = endTile;
    }

    public override void Perform(){
        base.Perform();
        entity.Move(endTile);
    }
}
