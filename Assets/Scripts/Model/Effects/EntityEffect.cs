using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEffect : Effect
{
    
    public Entity associatedEntity{
        get;
        set;
    }

    public EntityEffect(Entity entity, bool displayOnUI = true) : base(displayOnUI:displayOnUI){
        associatedEntity = entity;
    }

    public override bool CanBeActivated(){
        return associatedEntity != Entity.noEntity;
    }
}
