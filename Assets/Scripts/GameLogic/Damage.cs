using System;

namespace GameLogic{

    using GameState;
    [Serializable]
    public struct Damage{

        public int amount;

        public Damage(int amount){
            this.amount = amount;
        }

        public override string ToString()
        {
            return $"Damage with amount {amount}";
        }

        public static Damage operator+ (Damage d1, Damage d2) {
            
            return new Damage(d1.amount + d2.amount);
        }

        public static Damage operator- (Damage d1, Damage d2) {
            
            return new Damage(d1.amount - d2.amount);
        }

        public DamageState ToDamageState(){
            DamageState damageState = new DamageState();
            damageState.amount = amount;
            return damageState;
        }

    }
}