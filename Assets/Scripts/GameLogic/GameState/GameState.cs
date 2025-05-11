using System.Collections.Generic;
using UnityEngine;


namespace GameLogic{

    namespace GameState{
        public class GameState{
            
            public BoardState boardState{
                get;
                set;
            }

            public List<PlayerState> playerStates{
                get;
                set;
            }

            public uint currentPlayerNum{
                get;
                set;
            }

            public int turn{
                get;
                set;
            }

            public List<EffectState> effectStates{
                get;
                set;
            }
        }
    }
}

