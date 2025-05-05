using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameLogic;
using GameLogic.GameEffect;

[CreateAssetMenu(fileName = "EntityGivesAtkBuffWhenNextToEntitiesEffect", menuName = "Effect/PassiveEffect/EntityGivesAtkBuffWhenNextToEntitiesEffect", order = 0)]
public class ScriptableEntityGivesAtkBuffWhenNextToEntitiesEffect : ScriptableEffect{

    [SerializeField]
    public int amount;

    public override Effect GetEffect(){
        return new EntityGivesAtkBuffWhenNextToEntitiesEffect(amount, Entity.noEntity);
    }

}
