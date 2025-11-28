using System;

namespace GameLogic{

    namespace GameBuff{
        public class EffectDamageBuff : Buff
        {
            public Damage damage{
                get;
                private set;
            }

            public EffectDamageBuff(Damage damage, string assiociatedEffectId) : base("Effect Damage Buff", assiociatedEffectId){
                this.damage = damage;
            }

            public override string GetText(){
                return $"Effects damage {(Math.Sign(damage.amount) >= 0 ? "increased" : "decreased")} by {damage.amount}";
            }

            public override int IsPositive(){
                return Math.Sign(damage.amount);
            }
        }
    }
}
