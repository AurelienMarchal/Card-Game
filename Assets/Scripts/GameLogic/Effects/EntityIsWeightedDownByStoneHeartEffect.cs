using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic{

    using GameAction;
    using GameBuff;

    namespace GameEffect{
        //TODO : Become a GameEffect
        public class EntityIsWeightedDownByStoneHeartEffect : EntityEffect, GivesTempBuffInterface
        {

            List<Buff> Buffs; 

            public EntityIsWeightedDownByStoneHeartEffect(Entity entity) : base(entity, false)
            {
                Buffs = new List<Buff>();
            }

            public bool CheckTriggerToUpdateTempBuffs(Action action)
            {

                switch (action){
                    case StartGameAction startGameAction:
                        return startGameAction.wasPerformed;

                    case PlayerSpawnEntityAction playerSpawnEntityAction:
                        return playerSpawnEntityAction.entity == associatedEntity && playerSpawnEntityAction.wasPerformed;

                    case EntityTakeDamageAction entityTakeDamageAction:
                        return entityTakeDamageAction.entity == associatedEntity && entityTakeDamageAction.wasPerformed;
                    
                    case EntityPayHeartCostAction entityPayHeartCostAction:
                        return entityPayHeartCostAction.entity == associatedEntity && entityPayHeartCostAction.wasPerformed && (entityPayHeartCostAction.heartCost != null || entityPayHeartCostAction.heartCost.Length > 0) ; 

                    case EntityGainHeartAction entityGainHeartAction:
                        return entityGainHeartAction.entity == associatedEntity && entityGainHeartAction.heartType == HeartType.Stone && entityGainHeartAction.wasPerformed;
                }

                return false;
            }

            public System.Type[] ActionTypeTriggersToUpdateTempBuffs()
            {
                return new System.Type[5]{
                    typeof(StartGameAction), 
                    typeof(PlayerSpawnEntityAction), 
                    typeof(EntityTakeDamageAction), 
                    typeof(EntityPayHeartCostAction), 
                    typeof(EntityGainHeartAction)};
            }

            public List<Buff> GetTempBuffs()
            {
                return Buffs;
            }

            public void UpdateTempBuffs()
            {
                
                var stoneHeartCount = 0;
                foreach (var heart in associatedEntity.health.hearts){
                    if(heart == HeartType.Stone){
                        stoneHeartCount++;
                    }
                }

                Buffs.Clear();
                for (int i = 0; i < stoneHeartCount; i++){
                    Buffs.Add(new WeightedDownBuff(id.ToString()));
                }
                
            }
        }
    }
}