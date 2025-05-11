
namespace GameLogic{

    using GameState;

    namespace GameBuff{
        public class Buff{
            
            public string name{
                get;
                private set;
            }

            public Buff(string name) {
                this.name = name;
            }

            public virtual string GetText(){
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
                buffState.isPositive = buffState.isPositive;

                return buffState;
            }
        }
    }
}