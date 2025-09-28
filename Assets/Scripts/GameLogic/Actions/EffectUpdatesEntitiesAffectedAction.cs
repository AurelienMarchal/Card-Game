namespace GameLogic{

    using GameEffect;
    using GameLogic.GameState;

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
                    affectsEntitiesEffect.UpdateEntitiesAffected();
                    //if gives temp buff, update temp buff for entities
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
