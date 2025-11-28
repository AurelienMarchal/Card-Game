
namespace GameLogic{

    namespace GameEffect{
        using GameAction;
        
        public class UntilEndOfTurnEntityEffect : EntityEffect, CanBeActivatedInterface{
            public UntilEndOfTurnEntityEffect(Entity entity, bool displayOnUI = true) : base(entity, displayOnUI)
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
                        return playerEndTurnAction.wasPerformed;

                    default: return false;
                }
            }

            public System.Type[] ActionTypeTriggersToActivate()
            {
                return new System.Type[1]{typeof(PlayerEndTurnAction)};
            }

            void CanBeActivatedInterface.Activate()
            {
                Game.currentGame.PileAction(new RemoveEntityEffectFromEntityAction(associatedEntity, this));
            }
        }
    }
}