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
                        }

                        foreach (var previousAffectedEntity in previousAffectedEntities)
                        {
                            previousAffectedEntity.RemoveTempBuffByEffectId(effect.id.ToString());
                        }
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
