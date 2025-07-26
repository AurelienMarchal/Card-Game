using System.Collections.Generic;

namespace GameLogic{

    namespace GameState{
        public class EntityState{

            public uint num{
                get;
                set;
            }
            
            public uint playerNum{
                get;
                set;
            }

            public EntityModel model
            {
                get;
                set;
            }

            public string name{
                get;
                set;
            }

            public uint currentTileNum{
                get;
                set;
            }

            public HealthState healthState{
                get;
                set;
            }

            public Direction direction{
                get;
                set;
            }

            public int movementLeft{
                get;
                set;
            }

            public CostState costToAtkState{
                get;
                set;
            }

            public CostState baseCostToAtkState{
                get;
                set;
            }

            public int range{
                get;
                set;
            }

            public int baseRange{
                get;
                set;
            }

            public DamageState atkDamageState{
                get;
                set;
            }

            public DamageState baseAtkDamageState{
                get;
                set;
            }
            

            public int maxMovement{
                get;
                set;
            }

            public CostState costToMoveState{
                get;
                set;
            }
            
            public List<uint> tileNumsToMoveTo
            {
                get;
                set;
            }

            public List<EffectState> effectStates
            {
                get;
                set;
            }

            public List<BuffState> buffStates{
                get;
                set;
            }
        }
    }
}