using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ThrowProjectileEffect", menuName = "Effect/NonActivableEffect/ThrowProjectileEffect", order = 0)]
public class ScriptableThrowProjectileEffect : ScriptableEffect
{

    public int range;

    public Damage damage;

    public override Effect GetEffect()
    {
        return new ThrowProjectileEffect(Entity.noEntity, Direction.North, damage, range);
    }
}
