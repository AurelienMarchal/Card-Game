namespace GameLogic{

    using GameEffect;

    namespace GameAction{
        public abstract class EffectAction : Action
        {
            
            public Effect effect
            {
                get; private set;
            }

            //Targets

            public EffectAction(Effect effect, Action requiredAction = null) : base(requiredAction)
            {
                this.effect = effect;
            }
        }
    }
}
