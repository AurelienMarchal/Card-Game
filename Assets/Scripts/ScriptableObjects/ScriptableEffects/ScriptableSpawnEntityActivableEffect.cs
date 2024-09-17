using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpawnEntity", menuName = "Effect/ActivableEffect/SpawnEntity", order = 0)]
public class ScriptableSpawnEntityActivableEffect : ScriptableActivableEffect
{
    
    public ScriptableEntity scriptableEntity;

    public override ActivableEffect GetActivableEffect()
    {
        return new EntitySpawnEntityActivableEffect(Entity.noEntity, cost, scriptableEntity);
    }
}
