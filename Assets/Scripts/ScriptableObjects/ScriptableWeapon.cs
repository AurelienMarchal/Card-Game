
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoEffectWeapon", menuName = "Weapon/NoEffect Weapon", order = 0)]
public class ScriptableWeapon : ScriptableObject
{
    public Damage atkDamage;

    public Cost costToUse;

    public int range;

    public List<ScriptableEffect> scriptableEffects;

}