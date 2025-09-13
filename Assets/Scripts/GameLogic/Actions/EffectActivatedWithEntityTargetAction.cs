namespace GameLogic{

    using GameEffect;
    using GameLogic.GameState;

    namespace GameAction{
        public class EffectActivatedWithEntityTargetAction : Action
        {

            Effect effect;

            public Entity targetEntity
            {
                get; private set;
            }

            //Targets

            public EffectActivatedWithEntityTargetAction(Effect effect, Entity targetEntity, Action requiredAction = null) : base(requiredAction)
            {
                this.effect = effect;
                this.targetEntity = targetEntity;
            }
            protected override bool Perform()
            {
                if (effect is CanBeActivatedWithEntityTargetInterface canBeActivatedWithEntityTargetInterface)
                {
                    return canBeActivatedWithEntityTargetInterface.TryToActivateWithEntityTarget(targetEntity);
                }

                return effect.TryToActivate();
            }
            
            public override ActionState ToActionState()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}