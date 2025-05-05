using System.Collections.Generic;
using UnityEngine;
using GameLogic;


[CreateAssetMenu(fileName = "NoEffectMinion", menuName = "Entity/Minion/NoEffect Minion", order = 0)]
public class ScriptableEntity : ScriptableObject
{
    public Health health;

    public string entityName;

    public EntityModel entityModel;

    public int maxMovement;

    public List<ScriptableEffect> scriptableEffects;
    
}
