using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic{

    using GameAction;
    using GameBuff;

    namespace GameEffect{
        public class EntityIsWeightedDownByStoneHeartEffect : EntityEffect, GivesTempBuffInterface
        {

            List<Buff> entityBuffs; 

            public EntityIsWeightedDownByStoneHeartEffect(Entity entity) : base(entity, false)
            {
                entityBuffs = new List<Buff>();
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
                        return entityPayHeartCostAction.entity == associatedEntity && entityPayHeartCostAction.wasPerformed; 

                    case EntityGainHeartAction entityGainHeartAction:
                        return entityGainHeartAction.entity == associatedEntity && entityGainHeartAction.heartType == HeartType.Stone && entityGainHeartAction.wasPerformed;
                }

                return false;
            }

            public List<Buff> GetTempBuffs()
            {
                return entityBuffs;
            }

            public void UpdateTempBuffs()
            {
                
                var stoneHeartCount = 0;
                foreach (var heart in associatedEntity.health.hearts){
                    if(heart == HeartType.Stone){
                        stoneHeartCount++;
                    }
                }

                entityBuffs.Clear();
                for (int i = 0; i < stoneHeartCount; i++){
                    entityBuffs.Add(new WeightedDownBuff());
                }
                
            }
        }
    }
}