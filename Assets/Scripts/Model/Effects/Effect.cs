using System;
using System.Collections.Generic;
using UnityEngine;

public class Effect{
    
    public bool displayOnUI{
        get;
        private set;
    }

    public Effect(bool displayOnUI = true){
        this.displayOnUI = displayOnUI;
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

    public virtual bool TryToCreateEffectActivatedAction(Action requiredAction, out EffectActivatedAction effectActivatedAction){
        effectActivatedAction = new EffectActivatedAction(this, requiredAction);
        var canBeActivated = CanBeActivated();
        if(canBeActivated){
            this.effectActivatedAction = effectActivatedAction;
            Game.currentGame.PileAction(effectActivatedAction);
        }
        
        return canBeActivated;
    }

    public virtual bool Trigger(Action action){
        return false;
    }

    public virtual string GetEffectText(){
        return "";
    }

    public virtual void GetTilesAndEntitiesAffected(out Entity[] entitiesAffected, out Tile[] tilesAffected){

        tilesAffected = new Tile[0];
        entitiesAffected = new Entity[0];

    }
}
