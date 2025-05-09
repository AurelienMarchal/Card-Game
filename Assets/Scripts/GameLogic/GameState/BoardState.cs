using System.Collections.Generic;

namespace GameLogic{

    namespace GameState{
        public class BoardState{
            public int gridHeight{
                get;
                set;
            }

            public int gridWidth{
                get;
                set;
            }

            public List<TileState> tileStates{
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