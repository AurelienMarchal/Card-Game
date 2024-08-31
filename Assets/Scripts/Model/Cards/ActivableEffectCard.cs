using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableEffectCard : Card
{

    public ActivableEffect activableEffect{
        get;
        private set;
    }

    public ActivableEffectCard(Player player, string cardName, ActivableEffect activableEffect) : base(player, activableEffect.cost, cardName, "", false, true)
    {
        this.activableEffect = activableEffect;
    
    }

    public ActivableEffectCard(Player player, string cardName, ScriptableActivableEffect scriptableActivableEffect) : base(player, scriptableActivableEffect.cost, cardName, "", false, true){
        activableEffect = (ActivableEffect)scriptableActivableEffect.GetEffect();
    }

    public override bool CanBeActivated(Tile targetTile = null, Entity targetEntity = null)
    {
        activableEffect.associatedEntity = targetEntity;
        var canBeActivated = activableEffect.CanBeActivated();
        activableEffect.associatedEntity = Entity.noEntity;
        return canBeActivated;
    }

    protected override bool Activate(Tile targetTile = null, Entity targetEntity = null)
    {
        activableEffect.associatedEntity = targetEntity;
        return activableEffect.TryToCreateEffectActivatedAction(cardPlayedAction, out EffectActivatedAction _);
        
    }

    public override string GetText()
    {
        return activableEffect.GetEffectText();
    }

}
