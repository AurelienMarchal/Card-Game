using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    
    using GameEffect;
    using GameLogic.GameState;
    

    namespace GameAction
    {
        public class EffectUpdatesTempBuffsAction : EffectAction
        {

            public EffectUpdatesTempBuffsAction(Effect effect, Action requiredAction = null) : base(effect, requiredAction)
            {
            }
            protected override bool Perform()
            {

                var actionsToPile = new List<Action>();

                if (effect is GivesTempEntityBuffInterface givesTempBuffEffect)
                {
                    givesTempBuffEffect.UpdateTempEntityBuffs();
                    if (effect is AffectsEntitiesInterface affectsEntitiesEffect)
                    {
                        foreach (var affectedEntity in affectsEntitiesEffect.GetEntitiesAffected())
                        {
                            affectedEntity.RemoveTempBuffByEffectId(effect.id.ToString());
                            affectedEntity.AddTempBuffs(givesTempBuffEffect.GetTempEntityBuffs());

                            var atkIncreaseAccordingToBuffs = affectedEntity.GetAtkIncreaseAccordingToBuffs();

                            var atkDiff = affectedEntity.baseAtkDamage.amount + atkIncreaseAccordingToBuffs - affectedEntity.atkDamage.amount;

                            if (atkDiff != 0)
                            {
                                actionsToPile.Add(new EntityIncreasesAtkDamageAction(affectedEntity, atkDiff, this));
                            }

                            var rangeIncreaseAccordingToBuffs = affectedEntity.GetRangeIncreaseAccordingToBuffs();

                            var rangeDiff = affectedEntity.baseRange + rangeIncreaseAccordingToBuffs - affectedEntity.range;

                            if (rangeDiff != 0)
                            {
                                actionsToPile.Add(new EntityIncreasesRangeAction(affectedEntity, rangeDiff, this));

                            }

                            var mouvementCostToMoveIncreaseAccordingToBuffs = affectedEntity.GetMouvementCostToMoveIncreaseAccordingToBuffs();

                            var mouvementCostToMoveDiff = affectedEntity.baseCostToMove.mouvementCost + mouvementCostToMoveIncreaseAccordingToBuffs - affectedEntity.costToMove.mouvementCost;

                            if (mouvementCostToMoveDiff != 0)
                            {
                                actionsToPile.Add(new EntityIncreasesCostToMoveAction(affectedEntity, mouvementCostToMoveDiff, 0, null, this));
                            }

                            //same for hearts 
                            //same for cost to atk

                        }
                    }


                    Game.currentGame.PileActions(actionsToPile.ToArray());
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
