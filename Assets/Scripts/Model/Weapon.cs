
using System;
using System.Collections.Generic;

[Serializable]
public class Weapon{


    public string name{
        get;
        protected set;
    }

    public Damage atkDamage{
        get;
        protected set;
    }

    public Cost costToUse{
        get;
        protected set;
    }

    public int range{
        get;
        protected set;
    }

    public List<WeaponEffect> effects{
        get;
        protected set;
    }

    public const Weapon noWeapon = null;

    public Weapon(string name, Damage damage, Cost cost, int range, List<WeaponEffect> permanentEffects){
        this.name = name;
        atkDamage = damage;
        costToUse = cost;
        this.range = range;
        effects = new List<WeaponEffect>();
        AddEffectList(permanentEffects);
        AddDefaultPermanentEffects();
    }

    public Weapon(ScriptableWeapon scriptableWeapon){
        name = scriptableWeapon.weaponName;
        atkDamage = scriptableWeapon.atkDamage;
        costToUse = scriptableWeapon.costToUse;
        range = scriptableWeapon.range;
        effects = new List<WeaponEffect>();
        AddEffectList(scriptableWeapon.scriptableEffects);
        AddDefaultPermanentEffects();
    }

    public void AddEffectList(List<ScriptableEffect> scriptableEffects){
        foreach (var scriptableEffect in scriptableEffects){
            AddEffect(scriptableEffect);
        }
    }

    public void AddEffect(ScriptableEffect scriptableEffect){
        if (scriptableEffect.GetEffect() is WeaponEffect weaponEffect)
        {
            AddEffect(weaponEffect);
        }
    }

    public void AddEffect(WeaponEffect weaponEffect){
        weaponEffect.associatedWeapon = this;
        effects.Add(weaponEffect);
    }

    public void AddEffectList(List<WeaponEffect> weaponEffects){
        foreach (var effect in weaponEffects){
            AddEffect(effect);
        }
    }

    protected void AddDefaultPermanentEffects(){

    }

    public override string ToString()
    {
        return $"Weapon {name}";
    }

    
}