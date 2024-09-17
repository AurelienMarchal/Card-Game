using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoEffectActivableEffectCard", menuName = "Card/NoEffectActivableEffectCard", order = 0)]
public class ScriptableActivableEffectCard : ScriptableObject
{
    public Sprite sprite;

    public ScriptableActivableEffect scriptableActivableEffect;
}
