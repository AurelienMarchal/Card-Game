using System;
using System.Collections.Generic;
using UnityEngine;

public class Effect{

    public Player player{
        get;
        protected set;
    }

    public Effect(Player player){
        this.player = player;
    }

    protected EffectActivatedAction effectActivatedAction;

    public virtual bool CanBeActivated(){
        return false;
    }

    protected virtual void Activate(){
    }

    public bool TryToActivate(){
        var result = CanBeActivated();
        if(result){
            Activate();
        }
        return result;
    }

    public bool TryToCreateEffectActivatedAction(bool depile, Action requiredAction, out EffectActivatedAction effectActivatedAction){
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
}
