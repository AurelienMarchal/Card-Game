namespace GameLogic{

    using GameEffect;
    using GameLogic.GameState;

    namespace GameAction{
        public class EffectActivatesWithTileTargetAction : EffectAction
        {
            public Tile targetTile
            {
                get; private set;
            }

            public EffectActivatesWithTileTargetAction(Effect effect, Tile targetTile, Action requiredAction = null) : base(effect, requiredAction)
            {
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