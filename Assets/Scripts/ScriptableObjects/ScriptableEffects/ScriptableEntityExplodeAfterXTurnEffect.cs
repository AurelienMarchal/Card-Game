using UnityEngine;

[CreateAssetMenu(fileName = "EntityExplodeAfterXTurnEffect", menuName = "Effect/PassiveEffect/EntityExplodeAfterXTurnEffect", order = 0)]
public class ScriptableEntityExplodeAfterXTurnEffect : ScriptableEffect{

    public int turnNB;

    public int range;

    public Damage damage;

    public override Effect GetEffect(){
        return new EntityExplodeAfterXTurnEffect(turnNB, range, damage, Entity.noEntity);
    }

}
