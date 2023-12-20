using System;
using System.Collections.Generic;
using UnityEngine;

public class Effect{

    public Effect(){
        
    }

    protected EffectActivatedAction effectActivatedAction;

    public virtual bool CanBeActivated(){
        return false;
    }

    protected virtual void Activate(){
    }

    public virtual bool TryToActivate(){
        var result = CanBeActivated();
        if(result){
            Activate();
        }
        return result;
    }

    public virtual bool TryToCreateEffectActivatedAction(bool depile, Action requiredAction, out EffectActivatedAction effectActivatedAction){
        effectActivatedAction = new EffectActivatedAction(this, requiredAction);
        var canBeActivated = CanBeActivated();
        if(canBeActivated){
            this.effectActivatedAction = effectActivatedAction;
            Game.currentGame.PileAction(effectActivatedAction, depile);
        }
        
        return canBeActivated;
    }

    public virtual bool Trigger(Action action){
        return false;
    }

    public virtual string GetEffectText(){
        return "";
    }
}
