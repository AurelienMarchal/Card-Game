namespace GameLogic{

    using GameEffect;
    using GameLogic.GameState;

    namespace GameAction{
        public class EffectActivatedAction : Action
        {

            Effect effect;

            //Targets

            public EffectActivatedAction(Effect effect, Action requiredAction = null) : base(requiredAction)
            {
                this.effect = effect;
            }
            protected override bool Perform()
            {
                return effect.TryToActivate();
            }
            
            public override ActionState ToActionState()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}