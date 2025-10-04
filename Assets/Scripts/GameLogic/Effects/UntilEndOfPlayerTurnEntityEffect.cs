

namespace GameLogic{

    using GameAction;
    

    namespace GameEffect{
        public class UntilEndOfPlayerTurnEntityEffect : EntityEffect, CanBeActivatedInterface
        {
            public UntilEndOfPlayerTurnEntityEffect(Entity entity, bool displayOnUI = true) : base(entity, displayOnUI)
            {
            }

            public bool CanBeActivated()
            {
                return true;
            }

            public bool CheckTriggerToActivate(Action action)
            {
                switch (action){
                    case PlayerEndTurnAction playerEndTurnAction:
                        return associatedEntity.player == playerEndTurnAction.player;

                    default: return false;
                }
            }

            void CanBeActivatedInterface.Activate()
            {
                Game.currentGame.PileAction(new RemoveEntityEffectFromEntityAction(associatedEntity, this));
            }
        }
    }
}