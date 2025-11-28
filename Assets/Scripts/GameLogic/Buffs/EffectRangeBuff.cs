using System;

namespace GameLogic{

    namespace GameBuff{
        public class EffectRangeBuff : Buff
        {
            public int range{
                get;
                private set;
            }

            public EffectRangeBuff(int range, string assiociatedEffectId) : base("Effect Range Buff", assiociatedEffectId){
                this.range = range;
            }

            public override string GetText(){
                return $"Effects range {(Math.Sign(range) >= 0 ? "increased" : "decreased")} by {range}";
            }

            public override int IsPositive(){
                return Math.Sign(range);
            }
        }
    }
}
