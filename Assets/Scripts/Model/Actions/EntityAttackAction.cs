using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttackAction : EntityAction
{

    public Entity attackedEntity{
        get;
        protected set;
    }

    public bool isCounterAttack{
        get;
        protected set;
    }

    public EntityAttackAction(Entity attackingEntity, Entity attackedEntity, bool isCounterAttack = false, Action requiredAction = null) : base(attackingEntity, requiredAction){
        this.attackedEntity = attackedEntity;
    }

    protected override bool Perform()
    {

        if(entity.weapon == null){
            return false;
        }

        Game.currentGame.PileAction(new EntityTakeDamageAction(attackedEntity, entity.weapon.atkDamage, this));

        if(!isCounterAttack){
            if(attackedEntity.CanAttack(entity)){
                Game.currentGame.PileAction(new EntityAttackAction(attackedEntity, entity, false, this));
            }
        }
        

        return true;
    }
}
