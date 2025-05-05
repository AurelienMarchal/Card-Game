using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameLogic;
using GameLogic.GameEffect;


[CreateAssetMenu(fileName = "ThrowProjectileActivableEffect", menuName = "Effect/ActivableEffect/ThrowProjectileActivableEffect", order = 0)]
public class ScriptableThrowProjectileActivaleEffect : ScriptableActivableEffect
{
    public int range;

    public Damage damage;

    public override Effect GetEffect()
    {
        return new ThrowProjectileActivableEffect(Entity.noEntity, cost, damage, range);
    }

    public override ActivableEffect GetActivableEffect()
    {
        return new ThrowProjectileActivableEffect(Entity.noEntity, cost, damage, range);
    }
}
