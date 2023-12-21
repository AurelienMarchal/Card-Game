using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ActivableEffect", menuName = "Effect/ActivableEffect/NoEffect", order = 0)]
public class ScriptableActivableEffect : ScriptableEffect
{

    public Cost cost;

    public override Effect GetEffect()
    {
        return new ActivableEffect(Entity.noEntity, cost);
    }
}
