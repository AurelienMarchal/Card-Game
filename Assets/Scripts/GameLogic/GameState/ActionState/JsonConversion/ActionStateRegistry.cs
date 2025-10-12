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
            { "PlayerIncreaseMaxMana",      typeof(PlayerIncreaseMaxManaActionState)},
            { "PlayerUseMana",              typeof(PlayerUseManaActionState)},
            { "PlayerResetMana",            typeof(PlayerResetManaActionState)},
            { "PlayerPlayCard",             typeof(PlayerPlayCardActionState)},
            { "PlayerAddCardToHand",        typeof(PlayerAddCardToHandActionState) },
            { "PlayerDrawCard",             typeof(PlayerDrawCardActionState) },
            { "PlayerSpawnEntity",          typeof(PlayerSpawnEntityActionState) },

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
            { "EntityIncreasesAtkDamage",   typeof(EntityIncreasesAtkDamageActionState) },
            { "EntityIncreasesRange",       typeof(EntityIncreasesRangeActionState) },
            { "EntityIncreasesCostToAtk",   typeof(EntityIncreasesCostToAtkActionState) },
            { "EntityIncreasesCostToMove",  typeof(EntityIncreasesCostToMoveActionState) },

            { "TileChangeType",             typeof(TileChangeTypeActionState) },

        };
    }
}