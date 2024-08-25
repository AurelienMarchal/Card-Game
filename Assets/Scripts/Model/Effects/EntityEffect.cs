using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEffect : Effect
{
    
    public Entity associatedEntity{
        get;
        set;
    }

    public List<EntityBuff> entityBuffs{
        get;
        private set;
    }

    public EntityEffect(Entity entity, bool displayOnUI = true) : base(displayOnUI:displayOnUI){
        associatedEntity = entity;
        entityBuffs = new List<EntityBuff>();
    }

    public override bool CanBeActivated(){
        return associatedEntity != Entity.noEntity;
    }
}
