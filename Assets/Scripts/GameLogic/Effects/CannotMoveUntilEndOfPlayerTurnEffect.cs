

namespace GameLogic{
    using System.Collections.Generic;
    using GameBuff;
    using GameLogic.GameAction;

    namespace GameEffect{
        public class CannotMoveUntilEndOfPlayerTurnEffect : UntilEndOfPlayerTurnEntityEffect, AffectsEntitiesInterface, GivesTempBuffInterface{

            List<Buff> buffs;


            public CannotMoveUntilEndOfPlayerTurnEffect(Entity entity, bool displayOnUI = true) : base(entity, displayOnUI)
            {
                buffs = new List<Buff> { new EntityCannotMoveBuff() };
            }

            public bool CheckTriggerToUpdateEntitiesAffected(Action action)
            {
                return false;
            }

            public bool CheckTriggerToUpdateTempBuffs(Action action)
            {
                switch (action)
                {
                    case PlayerEndTurnAction playerEndTurnAction:
                        return playerEndTurnAction.wasPerformed && playerEndTurnAction.player == associatedEntity.player;
                }

                return false;
            }

            public override string GetEffectName()
            {
                return $"Cannot move until end of player turn";
            }

            public override string GetEffectText()
            {
                return $"{associatedEntity} cannot move until the end of the turn";
            }

            public List<Entity> GetEntitiesAffected()
            {
                return new List<Entity> { associatedEntity };
            }

            public List<Buff> GetTempBuffs()
            {
                return buffs;
            }

            public void UpdateEntitiesAffected()
            {
            }

            public void UpdateTempBuffs()
            {
                buffs.Clear();
                //finished
            }
        }
    }
}
