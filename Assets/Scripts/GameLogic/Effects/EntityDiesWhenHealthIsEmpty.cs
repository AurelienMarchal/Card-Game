

using UnityEngine;


namespace GameLogic{
    using System.Collections.Generic;
    using GameAction;

    namespace GameEffect{
        public class EntityDiesWhenHealthIsEmpty : Effect, CanBeActivatedInterface
        {

            List<Entity> entitiesWithEmptyHealth
            {
                get;
                set;
            }


            public EntityDiesWhenHealthIsEmpty() : base(displayOnUI:false)
            {
                entitiesWithEmptyHealth = new List<Entity>();
            }

            public override string GetEffectName()
            {
                return "Entities dies when health is empty";
            }

            public override string GetEffectText()
            {
                return GetEffectName();
            }

            public bool CanBeActivated()
            {
                return entitiesWithEmptyHealth.Count > 0;
            }


            public bool CheckTriggerToActivate(Action action)
            {   
                switch(action){
                    case EntityLooseHeartAction entityLooseHeartAction:
                        if (entityLooseHeartAction.entity.health.IsEmpty())
                        {
                            if (!entitiesWithEmptyHealth.Contains(entityLooseHeartAction.entity))
                            {
                                entitiesWithEmptyHealth.Add(entityLooseHeartAction.entity);
                            }
                            return true;
                        }
                        
                        return false;
                    case EntityPayHeartCostAction entityPayHeartCostAction:
                        if (entityPayHeartCostAction.entity.health.IsEmpty())
                        {
                            if (!entitiesWithEmptyHealth.Contains(entityPayHeartCostAction.entity))
                            {
                                entitiesWithEmptyHealth.Add(entityPayHeartCostAction.entity);
                            }
                            return true;
                        }
                        
                        return false;

                    case EntityTakeDamageAction entityTakeDamageAction:
                        if (entityTakeDamageAction.entity.health.IsEmpty())
                        {
                            if (!entitiesWithEmptyHealth.Contains(entityTakeDamageAction.entity))
                            {
                                entitiesWithEmptyHealth.Add(entityTakeDamageAction.entity);
                            }
                            return true;
                        }
                        
                        return false;
                    default : return false;
                }
            }

            public System.Type[] ActionTypeTriggersToActivate()
            {
                return new System.Type[3]{typeof(EntityLooseHeartAction), typeof(EntityPayHeartCostAction), typeof(EntityTakeDamageAction)};
            }

            public void Activate()
            {
                foreach (var entity in entitiesWithEmptyHealth)
                {
                    if (entity.health.IsEmpty())
                    {
                        Game.currentGame.PileAction(new EntityDieAction(entity));
                    }
                }

                entitiesWithEmptyHealth.Clear();
            }
        }
    }
}