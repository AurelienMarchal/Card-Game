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

    public virtual void Activate(bool depile){
        Game.currentGame.PileAction(new EffectActivatedAction(this), depile);
    }

    public virtual bool Trigger(Action action){
        return false;
    }
}
