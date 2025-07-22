using System;
using System.Collections.Generic;

namespace GameLogic.GameState
{
    public static class ActionStateRegistry
    {
        public static readonly Dictionary<string, Type> Types = new()
        {
            { "PlayerEndTurn", typeof(PlayerEndTurnActionState) },
            
        };
    }
}