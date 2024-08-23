using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDieAction : EntityAction
{
    public EntityDieAction(Entity entity, Action requiredAction = null) : base(entity, requiredAction){
        
    }
    

    protected override bool Perform()
    {
        Game.currentGame.board.entities.Remove(entity);
        return true;
    }
}
