
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Card{

    public bool needsEntityTarget{
        get;
        protected set;
    }

    public bool needsTileTarget{
        get;
        protected set;
    }

    public Player player{
        get;
        protected set;
    }

    public Cost cost{
        get;
        protected set;
    }

    public string cardName{
        get;
        protected set;
    }

    public string text{
        get;
        protected set;
    }

    protected CardPlayedAction cardPlayedAction;

    public Card(Player player, Cost cost, string cardName, string text, bool needsTileTarget = false, bool needsEntityTarget = false){
        this.player = player;
        this.cost = cost;
        this.cardName = cardName;
        this.text = text;
        this.needsTileTarget = needsTileTarget;
        this.needsEntityTarget = needsEntityTarget;
    }

    public Card(Player player, ScriptableCard scriptableCard){
        this.player = player;
        cost = scriptableCard.cost;
        cardName = scriptableCard.cardName;
        text = scriptableCard.text;
        needsTileTarget = scriptableCard.needsTileTarget;
        needsEntityTarget = scriptableCard.needsEntityTarget;
    }

    public bool TryToCreateCardPlayedAction(Action requiredAction, out CardPlayedAction cardPlayedAction, Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity){

        cardPlayedAction = new CardPlayedAction(this, requiredAction, targetTile, targetEntity);
        
        var canBeActivated = CanBeActivated();
        if(canBeActivated){
            this.cardPlayedAction = cardPlayedAction;
            Game.currentGame.PileAction(cardPlayedAction, true);
        }

        return canBeActivated;
    }

    public bool TryToActivate(Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity){
        var canBeActivated = CanBeActivated();
        if(canBeActivated){
            return Activate(targetTile, targetEntity);
        }
        return canBeActivated;
    }

    protected virtual bool Activate(Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity){
        return true;
    }

    public virtual bool CanBeActivated(Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity){
        return false;
    }

    public virtual List<Tile> PossibleTileTargets(){
        var tileTargetList = new List<Tile>();

        return tileTargetList;
    }

    public virtual List<Entity> PossibleEntityTargets(){
        var entityTargetList = new List<Entity>();

        return entityTargetList;
    }
}
