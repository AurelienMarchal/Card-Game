namespace GameLogic{

    using GameEffect;
    using GameLogic.GameState;

    namespace GameAction{
        public class EffectActivatesWithEntityTargetAction : EffectAction
        {
            public Entity targetEntity
            {
                get; private set;
            }

            //Targets

            public EffectActivatesWithEntityTargetAction(Effect effect, Entity targetEntity, Action requiredAction = null) : base(effect, requiredAction)
            {
                
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