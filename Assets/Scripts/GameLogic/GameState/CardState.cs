namespace GameLogic{

    namespace GameState{
        public class CardState{
            //TODO: Targets
            public bool needsEntityTarget{
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

            public EffectState activableEffectState{
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