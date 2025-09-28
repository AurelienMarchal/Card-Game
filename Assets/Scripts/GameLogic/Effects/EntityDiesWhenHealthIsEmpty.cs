

using UnityEngine;


namespace GameLogic{

    using GameAction;

    namespace GameEffect{
        public class EntityDiesWhenHealthIsEmpty : EntityEffect, CanBeActivatedInterface
        {
            public EntityDiesWhenHealthIsEmpty(Entity entity) : base(entity, displayOnUI:false)
            {
            
            }

            public override string GetEffectName()
            {
                return "Entity dies when health is empty";
            }

            public override string GetEffectText()
            {
                return GetEffectName();
            }

            public bool CanBeActivated()
            {
                return associatedEntity != Entity.noEntity;
            }


            public bool CheckTriggerToActivate(Action action)
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

            public void Activate()
            {
                Game.currentGame.PileAction(new EntityDieAction(associatedEntity));
            }
        }
    }
}