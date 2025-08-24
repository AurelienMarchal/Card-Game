using System.Collections.Generic;

namespace GameLogic{

    namespace GameState{
        public class CardState{
            
            public uint num
            {
                get;
                set;
            }

            public Dictionary<uint, List<uint>> possibleEntityTargets
            {
                get;
                set;
            }
            public List<uint> possibleTileTargets
            {
                get;
                set;
            }
            public bool needsEntityTarget
            {
                get;
                set;
            }

            public bool needsTileTarget{
                get;
                set;
            }

            public CostState costState{
                get;
                set;
            }

            public string text{
                get;
                set;
            }

            public string cardName{
                get;
                set;
            }
        }
    }
}