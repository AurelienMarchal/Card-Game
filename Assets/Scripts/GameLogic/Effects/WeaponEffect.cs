using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameLogic{

    namespace GameEffect{
        public class WeaponEffect : Effect{
            
            public Weapon associatedWeapon{
                get;
                private set;
            }

            public WeaponEffect(Weapon weapon, bool displayOnUI = true) : base(displayOnUI:displayOnUI){
                associatedWeapon = weapon;
            }

        }
    }
}