namespace GameLogic{

    using GameEffect;
    using GameLogic.GameState;

    namespace GameAction{
        public class EffectActivatedWithTileTargetAction : Action
        {

            Effect effect;

            public Tile targetTile
            {
                get; private set;
            }

            public EffectActivatedWithTileTargetAction(Effect effect, Tile targetTile, Action requiredAction = null) : base(requiredAction)
            {
                this.effect = effect;
                this.targetTile = targetTile;
            }
            protected override bool Perform()
            {
                if (effect is CanBeActivatedWithTileTargetInterface canBeActivatedWithTileTargetInterface)
                {
                    return canBeActivatedWithTileTargetInterface.TryToActivateWithTileTarget(targetTile);
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