using System;
using System.Collections.Generic;

namespace GameLogic.GameState
{
    public static class ActionStateRegistry
    {
        public static readonly Dictionary<string, Type> Types = new()
        {   
            { "StartGame",                  typeof(StartGameActionState) },
            { "StartTurn",                  typeof(StartTurnActionState) },

            { "PlayerStartTurn",            typeof(PlayerStartTurnActionState) },
            { "PlayerEndTurn",              typeof(PlayerEndTurnActionState) },
            { "PlayerAddCardToHand",        typeof(PlayerAddCardToHandActionState) },
            { "PlayerDrawCard",             typeof(PlayerDrawCardActionState) },
            
            { "EntityMove",                 typeof(EntityMoveActionState) },
            { "EntityChangeDirection",      typeof(EntityChangeDirectionActionState) },
            { "EntityAttack",               typeof(EntityAttackActionState) },
            { "EntityTakesDamage",          typeof(EntityTakesDamageActionState) },
            { "EntityUseMovement",          typeof(EntityUseMovementActionState) },
            { "EntityDie",                  typeof(EntityDieActionState) },
            { "EntityGainHeart",            typeof(EntityGainHeartActionState) },
            { "EntityHeals",                typeof(EntityHealsActionState) },
            { "EntityPayHeartCost",         typeof(EntityPayHeartCostActionState) },
            { "EntityResetMovement",        typeof(EntityResetMovementActionState) },
            { "EntityIncreaseMaxMovement",  typeof(EntityIncreaseMaxMovementActionState) },
            
            { "TileChangeType",             typeof(TileChangeTypeActionState) },
            
        };
    }
}