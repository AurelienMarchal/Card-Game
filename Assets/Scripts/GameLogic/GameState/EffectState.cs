using System.Collections.Generic;

namespace GameLogic{

    namespace GameState{

        //TODO: targets
        public class EffectState{

            public bool canBeActivated{
                get; 
                set;
            }

            public CostState costState{
                get;
                set;
            }

            public string effectText{
                get; 
                set;
            }

            public List<uint> entitiesAffectedNums{
                get; 
                set;
            }

            public List<uint> tilesAffectedNums{
                get;
                set;
            }
        }
    }
}