using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableEffect : Effect
{
    public Entity entity{
        get;
        protected set;
    }

    public Cost cost{
        get;
        protected set;
    }

    public bool costPaid{
        get;
        protected set;
    }

    public ActivableEffect(Entity associatedEntity, Cost cost){
        entity = associatedEntity;
        this.cost = cost;
    }

    public override bool CanBeActivated(){
        return entity.player.CanPayCost(cost) || costPaid;
    }

    public override bool TryToCreateEffectActivatedAction(bool depile, Action costAction, out EffectActivatedAction effectActivatedAction){
        effectActivatedAction = new EffectActivatedAction(this, costAction);
        costPaid = costAction.wasPerformed;
        var canBeActivated = CanBeActivated();
        if(canBeActivated){
            this.effectActivatedAction = effectActivatedAction;
            Game.currentGame.PileAction(effectActivatedAction, depile);
        }
        
        return canBeActivated;
    }
}
