

using UnityEngine;

public class EntityDiesWhenHealthIsEmpty : EntityEffect
{
    public EntityDiesWhenHealthIsEmpty(Entity entity) : base(entity)
    {
    
    }

    public override bool CanBeActivated()
    {
        return true;
    }


    public override bool Trigger(Action action)
    {   
        switch(action){
            case EntityLooseHeartAction entityLooseHeartAction:
                if(entityLooseHeartAction.entity == associatedEntity){
                    return associatedEntity.health.IsEmpty();
                }
                return false;
            

            case EntityPayHeartCostAction entityPayHeartCostAction:
                if(entityPayHeartCostAction.entity == associatedEntity){
                    return associatedEntity.health.IsEmpty();
                }
                return false;

            case EntityTakeDamageAction entityTakeDamageAction:
                if(entityTakeDamageAction.entity == associatedEntity){
                    return associatedEntity.health.IsEmpty();
                }
                return false;


            default : return false;
        }
    }

    protected override void Activate()
    {
        Game.currentGame.PileAction(new EntityDieAction(associatedEntity));
    }
}