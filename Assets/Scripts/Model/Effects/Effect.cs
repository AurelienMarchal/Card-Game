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

    public virtual bool CanBeActivated(){
        return false;
    }

    protected virtual void Activate(bool depile){
    }

    public bool TryToActivate(bool depile){
        var result = CanBeActivated();
        if(result){
            Activate(depile);
        }
        return result;
    }

    protected EffectActivatedAction PileEffectActivatedAction(bool depile){
        var effectActivatedAction = new EffectActivatedAction(this);
        Game.currentGame.PileAction(effectActivatedAction, depile);
        return effectActivatedAction;
    }

    public virtual bool Trigger(Action action){
        return false;
    }
}
