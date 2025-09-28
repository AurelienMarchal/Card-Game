namespace GameLogic{

    using GameEffect;
    using GameLogic.GameState;

    namespace GameAction{
        public class EffectUpdatesTilesAffectedAction : EffectAction
        {

            public EffectUpdatesTilesAffectedAction(Effect effect, Action requiredAction = null) : base(effect, requiredAction)
            {
            }
            protected override bool Perform()
            {
                if (effect is AffectsTilesInterface affectsTilesEffect)
                {
                    affectsTilesEffect.UpdateTilesAffected();
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
