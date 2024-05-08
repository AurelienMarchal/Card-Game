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

    public int manaLeft{
        get;
        private set;
    }

    public int maxMana{
        get;
        private set;
    }

    public const int maxManaCap = 10;

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

    public void ResetMana(){
        manaLeft = maxMana;
    }

    public bool TryToPlayCard(Card card, Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity){
        TryToCreatePayCostAction(card.cost, out PlayerPayCostAction playerPayCostAction);
        card.TryToCreateCardPlayedAction(playerPayCostAction, out CardPlayedAction cardPlayedAction, targetTile, targetEntity);
        return cardPlayedAction.wasPerformed;
    }

    public bool TryToActivateActivableEffect(ActivableEffect activableEffect){
        TryToCreatePayCostAction(activableEffect.cost, out PlayerPayCostAction playerPayCostAction);
        activableEffect.TryToCreateEffectActivatedAction(playerPayCostAction, out EffectActivatedAction effectActivatedAction);
        return effectActivatedAction.wasPerformed;
    }

    public bool TryToIncreaseMaxMana(){
        var canIncreaseMaxMana = CanIncreaseMaxMana();
        if(canIncreaseMaxMana){
            IncreaseMaxMana();
        }

        return canIncreaseMaxMana;
    }

    public bool CanIncreaseMaxMana(){
        return maxMana < maxManaCap;
    }

    private void IncreaseMaxMana(){
        maxMana = Math.Clamp(maxMana+1, 0, maxManaCap);
    }

    public bool TryToCreateSpawnEntityAction(EntityModel model, string name, Tile startingTile, Health startingHealth, int startingMaxMovement, Direction startingDirection, List<EntityEffect> permanentEffects, Action requiredAction, out PlayerSpawnEntityAction playerSpawnEntityAction, Weapon weapon=Weapon.noWeapon){
        playerSpawnEntityAction = new PlayerSpawnEntityAction(this, model, name, startingTile, startingHealth, startingMaxMovement, permanentEffects, startingDirection,  weapon, requiredAction);
        var canSpawnEntityAt = CanSpawnEntityAt(startingTile);
        if(canSpawnEntityAt){
            Game.currentGame.PileAction(playerSpawnEntityAction);
        }

        return canSpawnEntityAt;
    }

    public bool TryToCreateSpawnEntityAction(ScriptableEntity scriptableEntity, Tile startingTile, Direction startingDirection, Action requiredAction, out PlayerSpawnEntityAction playerSpawnEntityAction){
        playerSpawnEntityAction = new PlayerSpawnEntityAction(this, scriptableEntity,  startingTile, startingDirection,  requiredAction);
        var canSpawnEntityAt = CanSpawnEntityAt(startingTile);
        if(canSpawnEntityAt){
            Game.currentGame.PileAction(playerSpawnEntityAction);
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

            var useManaAction = new PlayerUseManaAction(this, cost.manaCost);
            var payHeartCostAction = new PlayerPayHeartCostAction(this, cost.heartCost);

            payCostAction = new PlayerPayCostAction(this, cost, payHeartCostAction);

            Game.currentGame.PileActions(new Action[]{payCostAction, useManaAction, payHeartCostAction});
            
        }
        return canPayHeartCost;
    }

    public bool CanPayCost(Cost cost){
        //TODO
        return CanPayHeartCost(cost.heartCost) && CanUseMana(cost.manaCost);
    }

    public bool TryToCreatePlayerUseManaAction(int mana, out PlayerUseManaAction useManaAction){
        useManaAction = new PlayerUseManaAction(this, mana);
        var canUseMana =  CanUseMana(mana);
        if(canUseMana){
            Game.currentGame.PileAction(useManaAction);
        }

        return canUseMana;
    }

    public bool TryToUseMana(int mana){
        
        var canUseMana = CanUseMana(mana);

        if(canUseMana){
            UseMouvement(mana);
        }

        return canUseMana;
    }

    private bool CanUseMana(int mana){
        return mana <= manaLeft && mana >= 0;
    }

    private void UseMouvement(int mana){
        manaLeft = Math.Clamp(manaLeft - mana, 0, maxMana);
        Debug.Log($"{this} using {mana} mana. {manaLeft} mana left");
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
        return hero.health.TryToPayHeartCost(hearts);
    }

    private bool CanPayHeartCost(HeartType[] hearts){
        return hero.health.CanPayHeartCost(hearts);
    }

    private void SetupPermanentEffects(){

    }

    public override string ToString(){
        return $"Player {playerNum}";
    }
}
