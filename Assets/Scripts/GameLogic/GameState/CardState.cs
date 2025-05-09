namespace GameLogic{

    namespace GameState{
        public class CardState{
            //TODO: Targets
            public bool needsEntityTarget{
                get;
                protected set;
            }

            public bool needsTileTarget{
                get;
                protected set;
            }

            public CostState costState{
                get;
                protected set;
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