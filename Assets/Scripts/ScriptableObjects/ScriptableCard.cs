using UnityEngine;

[CreateAssetMenu(fileName = "NoEffectCard", menuName = "Card/NoEffectCard", order = 0)]
public class ScriptableCard : ScriptableObject{

    public Cost cost;

    public string cardName;

    public string text;

    public Sprite sprite;

    public bool needsEntityTarget;

    public bool needsTileTarget;
}
