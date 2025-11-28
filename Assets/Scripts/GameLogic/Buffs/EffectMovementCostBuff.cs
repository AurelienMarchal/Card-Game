using System;

namespace GameLogic{

    namespace GameBuff{
        public class EffectMovementCostBuff : Buff
        {
            public int mouvement{
                get;
                private set;
            }

            public EffectMovementCostBuff(int mouvement, string assiociatedEffectId) : base("Effect Damage Buff", assiociatedEffectId){
                this.mouvement = mouvement;
            }

            public override string GetText(){
                return $"Effects cost {(Math.Sign(mouvement) >= 0 ? "increased" : "decreased")} by {mouvement} mouvement";
            }

            public override int IsPositive(){
                return Math.Sign(mouvement);
            }
        }
    }
}
