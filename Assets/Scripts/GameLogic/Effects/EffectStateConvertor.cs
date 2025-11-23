

namespace GameLogic
{
    using System.Collections.Generic;
    using GameState;

    namespace GameEffect
    {
        public static class EffectStateGenerator
        {
            public static EffectState GenerateEffectState(Effect effect)
            {
                EffectState effectState = new EffectState();
                effectState.effectText = effect.GetEffectText();
                effectState.effectName = effect.GetEffectName();
                effectState.displayOnUI = effect.displayOnUI;
                effectState.id = effect.id.ToString();
                if (effect is AffectsEntitiesInterface affectsEntitiesEffect)
                {
                    effectState.affectsEntities = true;
                    effectState.entitiesAffected = new Dictionary<uint, List<uint>>();
                    var entities = affectsEntitiesEffect.GetEntitiesAffected();
                    foreach (Entity entity in entities)
                    {
                        if (!effectState.entitiesAffected.ContainsKey(entity.player.playerNum))
                        {
                            effectState.entitiesAffected[entity.player.playerNum] = new List<uint>();
                        }
                        effectState.entitiesAffected[entity.player.playerNum].Add(entity.num);
                    }
                }
                else
                {
                    effectState.affectsEntities = false;
                }
                if (effect is AffectsTilesInterface affectsTilesInterface)
                {
                    effectState.affectsTiles = true;
                    effectState.tilesAffected = new List<uint>();
                    var tiles = affectsTilesInterface.GetTilesAffected();
                    foreach (Tile tile in tiles)
                    {
                        effectState.tilesAffected.Add(tile.num);
                    }
                }
                else
                {
                    effectState.affectsTiles = false;
                }

                if (effect is CanBeActivatedInterface canBeActivatedEffect)
                {
                    effectState.isActivableEffect = true;
                    effectState.canBeActivated = canBeActivatedEffect.CanBeActivated();
                }
                else
                {
                    effectState.isActivableEffect = false;
                    effectState.canBeActivated = false;
                }

                if (effect is CanBeActivatedWithEntityTargetInterface canBeActivatedWithEntityTargetEffect)
                {
                    effectState.isActivableWithEntityTargetEffect = true;
                    effectState.possibleEntityTargets = new Dictionary<uint, List<uint>>();
                    var entities = canBeActivatedWithEntityTargetEffect.PossibleEntityTargets();
                    foreach (Entity entity in entities)
                    {
                        if (!effectState.possibleEntityTargets.ContainsKey(entity.player.playerNum))
                        {
                            effectState.possibleEntityTargets[entity.player.playerNum] = new List<uint>();
                        }
                        effectState.possibleEntityTargets[entity.player.playerNum].Add(entity.num);
                    }
                }
                else
                {
                    effectState.isActivableWithEntityTargetEffect = false;
                }

                if (effect is CanBeActivatedWithTileTargetInterface canBeActivatedWithTileTargetEffect)
                {
                    effectState.isActivableWithTileTargetEffect = true;
                    effectState.possibleTileTargets = new List<uint>();
                    var tiles = canBeActivatedWithTileTargetEffect.PossibleTileTargets();
                    foreach (Tile tile in tiles)
                    {
                        effectState.possibleTileTargets.Add(tile.num);
                    }
                }
                else
                {
                    effectState.isActivableWithTileTargetEffect = false;
                }

                if (effect is GivesTempEntityBuffInterface givesTempEntityBuffEntity)
                {
                    effectState.givesTempEntityBuff = true;
                    //TODO
                }
                else
                {
                    effectState.givesTempEntityBuff = false;
                }

                if (effect is DealsDamageInterface dealsDamageEffect)
                {
                    effectState.dealsDamage = true;
                    effectState.damage = dealsDamageEffect.GetDamage().ToDamageState();
                }
                else
                {
                    effectState.dealsDamage = false;
                }

                if (effect is HasCostInterface hasCostEffect)
                {
                    effectState.hasCost = true;
                    effectState.cost = hasCostEffect.GetCost().ToCostState();

                    if (effect is EntityEffect entityEffect)
                    {
                        effectState.costCanBePaid = entityEffect.associatedEntity.CanPayCost(hasCostEffect.GetCost());
                    }
                    else
                    {
                        effectState.costCanBePaid = false;
                    }
                }
                else
                {
                    effectState.hasCost = false;
                    effectState.costCanBePaid = false;
                }

                if (effect is HasRangeInterface hasRangeEffect)
                {
                    effectState.hasRange = true;
                    effectState.range = hasRangeEffect.GetRange();
                }
                else
                {
                    effectState.dealsDamage = false;
                }

                return effectState;
            }
        }
    }
}
