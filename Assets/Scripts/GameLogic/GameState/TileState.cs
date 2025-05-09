using System.Collections.Generic;

namespace GameLogic{

    namespace GameState{
        public class TileState{
            public int gridX{
                get;
                set;
            }
            public int gridY{
                get;
                set;
            }

            public int num{
                get;
                set;
            }
            
            public TileType tileType{
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