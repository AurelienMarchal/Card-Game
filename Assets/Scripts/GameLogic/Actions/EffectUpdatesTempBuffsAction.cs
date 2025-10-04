namespace GameLogic{

    using GameEffect;
    using GameLogic.GameState;

    namespace GameAction{
        public class EffectUpdatesTempBuffsAction : EffectAction
        {

            public EffectUpdatesTempBuffsAction(Effect effect, Action requiredAction = null) : base(effect, requiredAction)
            {
            }
            protected override bool Perform()
            {
                if (effect is GivesTempEntityBuffInterface givesTempBuffEffect)
                {
                    givesTempBuffEffect.UpdateTempEntityBuffs();
                    if (effect is AffectsEntitiesInterface affectsEntitiesEffect)
                    {
                        foreach (var affectedEntity in affectsEntitiesEffect.GetEntitiesAffected())
                        {
                            affectedEntity.RemoveTempBuffByEffectId(effect.id.ToString());
                            affectedEntity.AddTempBuffs(givesTempBuffEffect.GetTempEntityBuffs());
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
