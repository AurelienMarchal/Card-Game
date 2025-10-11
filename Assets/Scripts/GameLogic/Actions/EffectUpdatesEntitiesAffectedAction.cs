namespace GameLogic{
    using System.Collections.Generic;
    using GameEffect;
    using GameLogic.GameState;
    using Unity.VisualScripting;

    namespace GameAction{
        public class EffectUpdatesEntitiesAffectedAction : EffectAction
        {

            public EffectUpdatesEntitiesAffectedAction(Effect effect, Action requiredAction = null) : base(effect, requiredAction)
            {
            }
            protected override bool Perform()
            {
                if (effect is AffectsEntitiesInterface affectsEntitiesEffect)
                {
                    //store last entities updated
                    var previousAffectedEntities = new List<Entity>(affectsEntitiesEffect.GetEntitiesAffected());

                    affectsEntitiesEffect.UpdateEntitiesAffected();

                    
                    //if gives temp buff, update temp buff for entities
                    if (effect is GivesTempEntityBuffInterface givesTempBuffEffect)
                    {

                        var actionsToPile = new List<Action>();
                        foreach (var currentAffectedEntity in affectsEntitiesEffect.GetEntitiesAffected())
                        {
                            if (!previousAffectedEntities.Contains(currentAffectedEntity))
                            {
                                currentAffectedEntity.AddTempBuffs(givesTempBuffEffect.GetTempEntityBuffs());

                            }
                            else
                            {
                                previousAffectedEntities.Remove(currentAffectedEntity);
                            }

                            var atkIncreaseAccordingToBuffs = currentAffectedEntity.GetAtkIncreaseAccordingToBuffs();

                            var atkDiff = currentAffectedEntity.baseAtkDamage.amount + atkIncreaseAccordingToBuffs - currentAffectedEntity.atkDamage.amount;

                            if (atkDiff != 0)
                            {
                                actionsToPile.Add(new EntityIncreasesAtkDamageAction(currentAffectedEntity, atkDiff, this));
                            }

                            var rangeIncreaseAccordingToBuffs = currentAffectedEntity.GetRangeIncreaseAccordingToBuffs();

                            var rangeDiff = currentAffectedEntity.baseRange + rangeIncreaseAccordingToBuffs - currentAffectedEntity.range;

                            if (rangeDiff != 0)
                            {
                                actionsToPile.Add(new EntityIncreasesRangeAction(currentAffectedEntity, rangeDiff, this));

                            }

                            var mouvementCostToMoveIncreaseAccordingToBuffs = currentAffectedEntity.GetMouvementCostToMoveIncreaseAccordingToBuffs();

                            var mouvementCostToMoveDiff = currentAffectedEntity.baseCostToMove.mouvementCost + mouvementCostToMoveIncreaseAccordingToBuffs - currentAffectedEntity.costToMove.mouvementCost;

                            if (mouvementCostToMoveDiff != 0)
                            {
                                actionsToPile.Add(new EntityIncreasesCostToMoveAction(currentAffectedEntity, mouvementCostToMoveDiff, 0, null, this));
                            }

                            //same for hearts 
                            //same for cost to atk
                        }

                        foreach (var previousAffectedEntity in previousAffectedEntities)
                        {
                            previousAffectedEntity.RemoveTempBuffByEffectId(effect.id.ToString());
                            var atkIncreaseAccordingToBuffs = previousAffectedEntity.GetAtkIncreaseAccordingToBuffs();

                            var atkDiff = previousAffectedEntity.baseAtkDamage.amount + atkIncreaseAccordingToBuffs - previousAffectedEntity.atkDamage.amount;

                            if (atkDiff != 0)
                            {
                                actionsToPile.Add(new EntityIncreasesAtkDamageAction(previousAffectedEntity, atkDiff, this));
                            }

                            var rangeIncreaseAccordingToBuffs = previousAffectedEntity.GetRangeIncreaseAccordingToBuffs();

                            var rangeDiff = previousAffectedEntity.baseRange + rangeIncreaseAccordingToBuffs - previousAffectedEntity.range;

                            if (rangeDiff != 0)
                            {
                                actionsToPile.Add(new EntityIncreasesRangeAction(previousAffectedEntity, rangeDiff, this));

                            }

                            var mouvementCostToMoveIncreaseAccordingToBuffs = previousAffectedEntity.GetMouvementCostToMoveIncreaseAccordingToBuffs();

                            var mouvementCostToMoveDiff = previousAffectedEntity.baseCostToMove.mouvementCost + mouvementCostToMoveIncreaseAccordingToBuffs - previousAffectedEntity.costToMove.mouvementCost;

                            if (mouvementCostToMoveDiff != 0)
                            {
                                actionsToPile.Add(new EntityIncreasesCostToMoveAction(previousAffectedEntity, mouvementCostToMoveDiff, 0, null, this));
                            }

                            //same for hearts 
                            //same for cost to atk
                        }

                        Game.currentGame.PileActions(actionsToPile.ToArray());
                    }
                    return true;
                }
                return false;
            }
            
            public override ActionState ToActionState()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
