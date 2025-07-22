namespace GameLogic{

    namespace GameEffect{

        using GameAction;

        public class UntilStartOfPlayerTurnEntityEffect : EntityEffect
        {
            public UntilStartOfPlayerTurnEntityEffect(Entity entity, bool displayOnUI = true) : base(entity, displayOnUI)
            {
            }


            public override bool Trigger(Action action)
            {
                switch (action){
                    case PlayerStartTurnAction playerStartTurnAction:
                        return associatedEntity.player == playerStartTurnAction.player;

                    default: return false;
                }
            }

            protected override void Activate()
            {
                Game.currentGame.PileAction(new RemoveEntityEffectFromEntityAction(associatedEntity, this, null));
            }
        }
    }
}