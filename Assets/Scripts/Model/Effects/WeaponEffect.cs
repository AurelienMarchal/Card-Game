using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffect : Effect
{
    
    public Weapon associatedWeapon{
        get;
        set;
    }

    public WeaponEffect(Weapon weapon){
        associatedWeapon = weapon;
    }

    public override bool CanBeActivated(){
        return associatedWeapon != Weapon.noWeapon;
    }
    

}