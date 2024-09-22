using UnityEngine;


[CreateAssetMenu(fileName = "EntityHealsActivableEffect", menuName = "Effect/ActivableEffect/EntityHealsActivableEffect", order = 0)]
public class ScriptableEntityHealsActivableEffect : ScriptableActivableEffect{
    
    public int numberOfHeartsHealed;

    public override ActivableEffect GetActivableEffect(){
        return new EntityHealsActivableEffect(numberOfHeartsHealed, Entity.noEntity, cost);
    }
    
}
