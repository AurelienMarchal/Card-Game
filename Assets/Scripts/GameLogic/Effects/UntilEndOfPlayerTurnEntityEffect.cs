

namespace GameLogic{

    using GameAction;
    

    namespace GameEffect{
        public class UntilEndOfPlayerTurnEntityEffect : EntityEffect
        {
            public UntilEndOfPlayerTurnEntityEffect(Entity entity, bool displayOnUI = true) : base(entity, displayOnUI)
            {
            }

            public override bool Trigger(Action action)
            {
                switch (action){
                    case EndPlayerTurnAction endPlayerTurnAction:
                        return associatedEntity.player == endPlayerTurnAction.player;

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