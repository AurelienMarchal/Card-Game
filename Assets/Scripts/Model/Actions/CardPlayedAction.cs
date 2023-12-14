using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardPlayedAction : CardAction
{
    public Tile targetTile{
        get;
        protected set;
    }

    public Entity targetEntity{
        get;
        protected set;
    }

    public CardPlayedAction(Card card, Action requiredAction, Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity): base(card, requiredAction){
        this.targetTile = targetTile;
        this.targetEntity = targetEntity;
    }

    protected override bool Perform(){
        return card.TryToActivate(targetTile, targetEntity);
    }
}
