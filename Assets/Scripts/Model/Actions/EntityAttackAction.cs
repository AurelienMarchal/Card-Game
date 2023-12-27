using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttackAction : EntityAction
{

    public Entity attackedEntity{
        get;
        protected set;
    }

    public EntityAttackAction(Entity attackingEntity, Entity attackedEntity, Action requiredAction = null) : base(attackingEntity, requiredAction){
        this.attackedEntity = attackedEntity;
    }

    protected override bool Perform()
    {
        if(entity.hasAttacked){
            return false;
        }

        Game.currentGame.PileAction(new EntityTakeDamageAction(attackedEntity, entity.atkDamage, this));
        Game.currentGame.PileAction(new EntityTakeDamageAction(entity, attackedEntity.atkDamage, this));

        entity.hasAttacked = true;

        return true;
    }
}
