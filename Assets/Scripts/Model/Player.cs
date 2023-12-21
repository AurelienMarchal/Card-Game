using System;
using System.Collections.Generic;
using UnityEngine;

public class Player{

    public List<Entity> entities{
        get;
        private set;
    }

    public int playerNum{
        get;
        private set;
    }

    public int playerColor{
        get;
        private set;
    }

    public int movementLeft{
        get;
        private set;
    }

    public int maxMovement{
        get;
        private set;
    }

    public const int maxMovementCap = 10;

    public Hero hero{
        get;
        set;
    }

    public Hand hand{
        get;
        private set;
    }

    public List<Effect> effects{
        get;
        protected set;
    }

    public Player(int num, int color){
        playerNum = num;
        playerColor = color;
        hand = new Hand(this);
        entities = new List<Entity>();
        effects = new List<Effect>();
        SetupPermanentEffects();
    }

    public void ResetMovement(){
        movementLeft = maxMovement;
    }

    public bool TryToPlayCard(Card card, Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity){
        TryToCreatePayCostAction(card.cost, out PlayerPayCostAction playerPayCostAction);
        card.TryToCreateCardPlayedAction(playerPayCostAction, out CardPlayedAction cardPlayedAction, targetTile, targetEntity);
        return cardPlayedAction.wasPerformed;
    }

    public bool TryToActivateActivableEffect(ActivableEffect activableEffect){
        TryToCreatePayCostAction(activableEffect.cost, out PlayerPayCostAction playerPayCostAction);
        activableEffect.TryToCreateEffectActivatedAction(true, playerPayCostAction, out EffectActivatedAction effectActivatedAction);
        return effectActivatedAction.wasPerformed;
    }

    public bool TryToIncreaseMaxMovement(){
        if(maxMovement >= maxMovementCap){
            maxMovement = Math.Clamp(maxMovement, 0, maxMovementCap);
            return false;
        }
        else{
            maxMovement ++;
            maxMovement = Math.Clamp(maxMovement, 0, maxMovementCap);
            return true;
        }
        
    }

    public bool TryToCreateSpawnEntityAction(EntityModel model, string name, Tile startingTile, Health startingHealth, Damage startingDamageAtk, Direction startingDirection, List<EntityEffect> permanentEffects, Action requiredAction, out PlayerSpawnEntityAction playerSpawnEntityAction){
        playerSpawnEntityAction = new PlayerSpawnEntityAction(this, model, name, startingTile, startingHealth, startingDamageAtk, permanentEffects, startingDirection,  requiredAction);
        var canSpawnEntityAt = CanSpawnEntityAt(startingTile);
        if(canSpawnEntityAt){
            Game.currentGame.PileAction(playerSpawnEntityAction, false);
        }

        return canSpawnEntityAt;
    }

    public bool TryToCreateSpawnEntityAction(ScriptableEntity scriptableEntity, Tile startingTile, Direction startingDirection, Action requiredAction, out PlayerSpawnEntityAction playerSpawnEntityAction){
        playerSpawnEntityAction = new PlayerSpawnEntityAction(this, scriptableEntity,  startingTile, startingDirection,  requiredAction);
        var canSpawnEntityAt = CanSpawnEntityAt(startingTile);
        if(canSpawnEntityAt){
            Game.currentGame.PileAction(playerSpawnEntityAction, false);
        }

        return canSpawnEntityAt;
    }

    public bool TryToSpawnEntity(Entity entity){
        var canSpawnEntity = CanSpawnEntity(entity);
        if(canSpawnEntity){
            SpawnEntity(entity);
        }
        return canSpawnEntity;
    }

    private void SpawnEntity(Entity entity){
        entities.Add(entity);
    }

    public bool CanSpawnEntity(Entity entity){
        return Game.currentGame.board.GetEntityAtTile(entity.currentTile) == Entity.noEntity;
    }

    public bool CanSpawnEntityAt(Tile tile){
        return Game.currentGame.board.GetEntityAtTile(tile) == Entity.noEntity;
    }

    public bool TryToCreatePayCostAction(Cost cost, out PlayerPayCostAction payCostAction){
        payCostAction = new PlayerPayCostAction(this, cost);
        var canPayHeartCost = CanPayCost(cost);
        if(canPayHeartCost){

            var useMovementAction = new PlayerUseMovementAction(this, cost.movementCost);
            var payHeartCostAction = new PlayerPayHeartCostAction(this, cost.heartCost);

            payCostAction = new PlayerPayCostAction(this, cost, payHeartCostAction);

            Game.currentGame.PileAction(payCostAction, false);
            Game.currentGame.PileAction(useMovementAction, false);
            Game.currentGame.PileAction(payHeartCostAction, true);
            
        }
        return canPayHeartCost;
    }

    public bool CanPayCost(Cost cost){
        //TODO
        return CanPayHeartCost(cost.heartCost) && CanUseMovement(cost.movementCost);
    }

    public bool TryToCreatePlayerUseMovementAction(int movement, out PlayerUseMovementAction useMovementAction){
        useMovementAction = new PlayerUseMovementAction(this, movement);
        var canUseMovement =  CanUseMovement(movement);
        if(canUseMovement){
            Game.currentGame.PileAction(useMovementAction);
        }

        return canUseMovement;
    }

    public bool TryToUseMovement(int movement){
        
        var canUseMovement = CanUseMovement(movement);

        if(canUseMovement){
            UseMouvement(movement);
        }

        return canUseMovement;
    }

    private bool CanUseMovement(int movement){
        if(movement > movementLeft){
            Debug.Log($"{this} cannot use {movement} movement. {movementLeft} movement left");
            return false;
        }

        return true;
    }

    private void UseMouvement(int movement){
        movementLeft -= movement;
        Debug.Log($"{this} using {movement} movement. {movementLeft} movement left");
    }

    public bool TryToCreatePayHeartCostAction(HeartType[] hearts, out PlayerPayHeartCostAction payHeartCostAction){
        payHeartCostAction = new PlayerPayHeartCostAction(this, hearts);
        var canPayHeartCost = CanPayHeartCost(hearts);
        if(canPayHeartCost){
            Game.currentGame.PileAction(payHeartCostAction);
        }

        return canPayHeartCost;
    }

    public bool TryToPayHeartCost(HeartType[] hearts){
        
        var canPayHeartCost = CanPayHeartCost(hearts);

        if(canPayHeartCost){
            PayHeartCost(hearts);
        }

        return canPayHeartCost;
    }

    private bool CanPayHeartCost(HeartType[] hearts){
        //TODO
        return true;
    }

    private void PayHeartCost(HeartType[] hearts){
        Debug.Log($"{this} paying {hearts}");
    }

    private void SetupPermanentEffects(){

    }

    public override string ToString(){
        return $"Player {playerNum}";
    }
}
