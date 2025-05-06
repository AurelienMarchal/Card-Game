using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic{

    using GameAction;
    using GameBuff;

    namespace GameEffect{
        public class EntityIsWeightedDownByStoneHeartEffect : EntityEffect
        {
            public EntityIsWeightedDownByStoneHeartEffect(Entity entity) : base(entity, false)
            {
            
            
            }

            public override bool Trigger(Action action)
            {

                switch (action){
                    case StartGameAction startGameAction:
                        return startGameAction.wasPerformed;

                    case PlayerSpawnEntityAction playerSpawnEntityAction:
                        return playerSpawnEntityAction.entitySpawned == associatedEntity && playerSpawnEntityAction.wasPerformed;

                    case EntityTakeDamageAction entityTakeDamageAction:
                        return entityTakeDamageAction.entity == associatedEntity && entityTakeDamageAction.wasPerformed;
                    
                    case EntityPayHeartCostAction entityPayHeartCostAction:
                        return entityPayHeartCostAction.entity == associatedEntity && entityPayHeartCostAction.wasPerformed; 

                    case EntityGainHeartAction entityGainHeartAction:
                        return entityGainHeartAction.entity == associatedEntity && entityGainHeartAction.heartType == HeartType.Stone && entityGainHeartAction.wasPerformed;
                }

                return false;
            }


            protected override void Activate()
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

                if(!associatedEntity.affectedByEffects.Contains(this)){
                    associatedEntity.affectedByEffects.Add(this);
                }
                
                associatedEntity.UpdateTempBuffsAccordingToEffects();
            }

            public override void GetTilesAndEntitiesAffected(out Entity[] entitiesAffected, out Tile[] tilesAffected){
                tilesAffected = new Tile[0];
                entitiesAffected = new Entity[1]{associatedEntity};
            }
        }
    }
}