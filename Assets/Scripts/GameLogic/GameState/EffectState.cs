using System.Collections.Generic;

namespace GameLogic{

    namespace GameState{

        //TODO: targets
        public class EffectState{

            public bool canBeActivated{
                get; 
                set;
            }
            
            //add displayOnUI

            public string effectText{
                get; 
                set;
            }

            //TODO : dictionnary by player num
            public List<uint> entitiesAffectedNums
            {
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