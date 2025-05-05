
namespace GameLogic{

    namespace GameEffect{
        using GameAction;
        
        public class UntilEndOfTurnEntityEffect : EntityEffect{
            public UntilEndOfTurnEntityEffect(Entity entity, bool displayOnUI = true) : base(entity, displayOnUI)
            {
            }

            protected override void Activate()
            {
                Game.currentGame.PileAction(new RemoveEntityEffectFromEntityAction(associatedEntity, this, null));
            }
        }
    }
}