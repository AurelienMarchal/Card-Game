namespace GameLogic{

    namespace GameEffect{

        using GameAction;

        public class UntilStartOfPlayerTurnEntityEffect : EntityEffect, CanBeActivatedInterface
        {
            public UntilStartOfPlayerTurnEntityEffect(Entity entity, bool displayOnUI = true) : base(entity, displayOnUI)
            {
            }

            public bool CanBeActivated()
            {
                return true;
            }

            public bool CheckTriggerToActivate(Action action)
            {
                switch (action){
                    case PlayerStartTurnAction playerStartTurnAction:
                        return playerStartTurnAction.wasPerformed && associatedEntity.player == playerStartTurnAction.player;

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