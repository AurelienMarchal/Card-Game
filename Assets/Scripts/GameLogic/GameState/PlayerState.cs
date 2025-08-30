using System.Collections.Generic;

namespace GameLogic{

    namespace GameState{
        public class PlayerState{
            public List<EntityState> entityStates{
                get;
                set;
            }

            public uint playerNum{
                get;
                set;
            }

            public HeroState heroState{
                get;
                set;
            }

            public HandState handState{
                get;
                set;
            }
            
            public int manaLeft{
                get;
                set;
            }

            public int maxMana{
                get;
                set;
            }

            public int maxManaCap {
                get;
                set;
            }

            public List<EffectState> effectStates
            {
                get;
                set;
            }
        }
    }
}