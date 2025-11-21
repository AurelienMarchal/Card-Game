
namespace GameLogic{

    using GameState;
    using UnityEditor;

    namespace GameBuff{
        public class Buff{
            
            public string name{
                get;
                private set;
            }
            
            public string assiociatedEffectId{
                get;
                private set;
            }
            
            public Buff(string name, string assiociatedEffectId)
            {
                this.name = name;
                this.assiociatedEffectId = assiociatedEffectId;
            }

            public virtual string GetText()
            {
                return string.Empty;
            }

            public override string ToString() {
                return name + "buff";
            }

            public virtual int IsPositive(){
                return 0;
            }

            public BuffState ToBuffState(){
                BuffState buffState = new BuffState();
                buffState.name = name;
                buffState.text = GetText();
                buffState.isPositive = IsPositive();

                return buffState;
            }
        }
    }
}