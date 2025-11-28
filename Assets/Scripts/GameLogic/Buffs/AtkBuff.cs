using System;

namespace GameLogic{

    namespace GameBuff{
        public class AtkBuff : Buff
        {
            public int amount{
                get;
                private set;
            }

            public AtkBuff(int amount, string assiociatedEffectId) : base("Atk Buff", assiociatedEffectId){
                this.amount = amount;
            }

            public override string GetText(){
                return $"Atk {(Math.Sign(amount) >= 0 ? "increased" : "decreased")} by {amount}";
            }

            public override int IsPositive(){
                return Math.Sign(amount);
            }
        }
    }
}
