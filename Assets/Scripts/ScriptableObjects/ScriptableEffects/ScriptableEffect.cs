using UnityEngine;

using GameLogic.GameEffect;

[CreateAssetMenu(fileName = "NoEffect", menuName = "Effect/NonActivableEffect/NoEffect", order = 0)]
public class ScriptableEffect : ScriptableObject
{

    public virtual Effect GetEffect(){
        return new Effect();
    }

}
