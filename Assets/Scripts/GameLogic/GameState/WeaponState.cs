using System.Collections.Generic;

namespace GameLogic{

    namespace GameState{
        public class WeaponState{

            public string name{
                get;
                set;
            }

            public DamageState atkDamageState{
                get;
                set;
            }

            public CostState costToUseState{
                get;
                set;
            }

            public int range{
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