namespace GameLogic{

    using GameEffect;
    using GameLogic.GameState;

    namespace GameAction{
        public class EffectActivatesAction : EffectAction
        {

            public EffectActivatesAction(Effect effect, Action requiredAction = null) : base(effect, requiredAction)
            {
            }
            protected override bool Perform()
            {
                if (effect is CanBeActivatedInterface canBeActivatedEffect)
                {
                    return canBeActivatedEffect.TryToActivate();
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