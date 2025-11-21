using System;
using System.Collections.Generic;

namespace GameLogic{

    using GameState;

    namespace GameEffect{
        //TODO: Effects with targets
        public abstract class Effect{
            
            protected Dictionary<string, object> metaData
            {
                get;
                set;
            }

            public Guid id{
                get;
                private set;
            }

            public bool displayOnUI
            {
                get;
                private set;
            }

            public Effect(bool displayOnUI = true)
            {
                this.displayOnUI = displayOnUI;
                id = Guid.NewGuid();
                metaData = new Dictionary<string, object>();
            }

            public Effect(EffectState effectState)
            {
                metaData = effectState.metaData;
                displayOnUI = effectState.displayOnUI;
                id = new Guid(effectState.id);
            }

            public virtual string GetEffectName()
            {
                return "No Name";
            }
            
            public virtual string GetEffectText()
            {
                return "No Effect";
            }

            
        }
    }
}