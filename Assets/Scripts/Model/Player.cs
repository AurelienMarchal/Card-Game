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

    public bool TryToPlayCard(
        Card card,
        Dictionary<Entity, int> mouvementCostDistribution,
        Dictionary<Entity, HeartType[]> heartCostDistribution,
        Tile targetTile = Tile.noTile, 
        Entity targetEntity = Entity.noEntity
    ){
        TryToCreatePayCostAction(card.cost, mouvementCostDistribution, heartCostDistribution, out PlayerPayCostAction playerPayCostAction);
        card.TryToCreateCardPlayedAction(playerPayCostAction, out CardPlayedAction cardPlayedAction, targetTile, targetEntity);
        return cardPlayedAction.wasPerformed;
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

    public bool TryToCreatePayCostAction(
        Cost cost, 
        Dictionary<Entity, int> mouvementCostDistribution,
        Dictionary<Entity, HeartType[]> heartCostDistribution,
        out PlayerPayCostAction payCostAction){
        payCostAction = new PlayerPayCostAction(this, cost, mouvementCostDistribution, heartCostDistribution);
        var canPayCost = CanPayCost(cost, mouvementCostDistribution, heartCostDistribution);
        if(canPayCost){

            var didPayMouvementCost = TryToCreatePayMouvementCostAction(cost.mouvementCost, mouvementCostDistribution, out PlayerPayMouvementCostAction playerPayMouvementCostAction);
            var didPayHeartCost = TryToCreatePayHeartCostAction(cost.heartCost, heartCostDistribution, out PlayerPayHeartCostAction playerPayHeartCostAction);

            if(!didPayMouvementCost || !didPayHeartCost){
                return false;
            }

            payCostAction = new PlayerPayCostAction(this, cost, mouvementCostDistribution, heartCostDistribution);
            Game.currentGame.PileAction(payCostAction);
            
        }
        return canPayCost;
    }

    public bool CanPayCost(
        Cost cost,
        Dictionary<Entity, int> mouvementCostDistribution,
        Dictionary<Entity, HeartType[]> heartCostDistribution
        ){
        
        return CanPayMouvementCost(cost.mouvementCost, mouvementCostDistribution) && CanPayHeartCost(cost.heartCost, heartCostDistribution);
    }

    [Obsolete]
    public bool TryToCreatePlayerUseManaAction(int mana, out PlayerUseManaAction useManaAction){
        useManaAction = new PlayerUseManaAction(this, mana);
        var canUseMana =  CanUseMana(mana);
        if(canUseMana){
            Game.currentGame.PileAction(useManaAction);
        }

        return canUseMana;
    }

    [Obsolete]
    public bool TryToUseMana(int mana){
        
        var canUseMana = CanUseMana(mana);

        if(canUseMana){
            UseMana(mana);
        }

        return canUseMana;
    }

    [Obsolete]
    private bool CanUseMana(int mana){
        return mana <= manaLeft && mana >= 0;
    }

    [Obsolete]
    private void UseMana(int mana){
        manaLeft = Math.Clamp(manaLeft - mana, 0, maxMana);
        Debug.Log($"{this} using {mana} mana. {manaLeft} mana left");
    }

    public bool TryToCreatePayHeartCostAction(HeartType[] hearts, Dictionary<Entity, HeartType[]> heartCostDistribution, out PlayerPayHeartCostAction payHeartCostAction){
        payHeartCostAction = new PlayerPayHeartCostAction(this, hearts, heartCostDistribution);
        var canPayHeartCost = CanPayHeartCost(hearts, heartCostDistribution);
        if(canPayHeartCost){

            var heartCostCopy = new List<HeartType>(hearts);
            var requiredActionList = new List<Action>();

            foreach(var entry in heartCostDistribution){
                if(!entry.Key.CanPayHeartCost(entry.Value)){
                    return false;
                }
                foreach(var heartType in entry.Value){
                    var result = TryToCreatePlayerUseEntityHeartsAction(entry.Key, entry.Value, out PlayerUseEntityHeartsAction playerUseEntityHeartsAction);
                    if(!result){
                        return false;
                    }
                    requiredActionList.Add(playerUseEntityHeartsAction);

                    if(heartCostCopy.Contains(heartType)){
                        heartCostCopy.Remove(heartType);
                    }
                }
            }

            if(heartCostCopy.Count == 0 && !(requiredActionList.Count == 0)){
                payHeartCostAction = new PlayerPayHeartCostAction(this, hearts, heartCostDistribution, requiredActionList[^1]);
                Game.currentGame.PileAction(payHeartCostAction);
            }            
        }

        return canPayHeartCost;
    }

    public bool CanPayHeartCost(HeartType[] hearts, Dictionary<Entity, HeartType[]> heartCostDistribution){

        var heartCostCopy = new List<HeartType>(hearts);
        foreach(var entry in heartCostDistribution){
            if(!entry.Key.CanPayHeartCost(entry.Value)){
                return false;
            }
            foreach(var heartType in entry.Value){
                if(heartCostCopy.Contains(heartType)){
                    heartCostCopy.Remove(heartType);
                }
            }
        }
        return heartCostCopy.Count == 0;
    }

    public bool TryToCreatePayMouvementCostAction(int mouvementCost, Dictionary<Entity, int> mouvementCostDistribution, out PlayerPayMouvementCostAction payMouvementCostAction){
        payMouvementCostAction = new PlayerPayMouvementCostAction(this, mouvementCost, mouvementCostDistribution);
        var canPayMouvementCost = CanPayMouvementCost(mouvementCost, mouvementCostDistribution);
        if(canPayMouvementCost){
            var mouvementToPay = mouvementCost;
            var requiredActionList = new List<Action>();

            foreach(var entry in mouvementCostDistribution){
                if(!entry.Key.CanUseMovement(entry.Value)){
                    return false;
                }
                var result = TryToCreatePlayerUseEntityMouvementAction(entry.Key, entry.Value, out PlayerUseEntityMouvementAction useEntityMouvementAction);
                if(!result){
                    return false;
                }
                requiredActionList.Add(useEntityMouvementAction);
                mouvementToPay -= entry.Value;
            }

            if(mouvementToPay == 0 && !(requiredActionList.Count == 0)){
                payMouvementCostAction = new PlayerPayMouvementCostAction(this, mouvementCost, mouvementCostDistribution, requiredActionList[^1]);
                Game.currentGame.PileAction(payMouvementCostAction);
            }
        }

        return canPayMouvementCost;
    }

    public bool CanPayMouvementCost(int mouvementCost, Dictionary<Entity, int> mouvementCostDistribution){

        var mouvementToPay = mouvementCost;

        foreach(var entry in mouvementCostDistribution){
            if(!entry.Key.CanUseMovement(entry.Value)){
                return false;
            }
            mouvementToPay -= entry.Value;
        }
        return mouvementToPay == 0;
    }

    public bool TryToCreatePlayerUseEntityMouvementAction(Entity entity, int mouvement, out PlayerUseEntityMouvementAction useEntityMouvementAction){
        useEntityMouvementAction = new PlayerUseEntityMouvementAction(this, entity, mouvement);
        var canUseEntityMouvement =  CanUseEntityMouvement(entity, mouvement);
        if(canUseEntityMouvement){
            entity.TryToCreateEntityUseMovementAction(mouvement, out EntityUseMovementAction entityUseMovementAction);
            useEntityMouvementAction = new PlayerUseEntityMouvementAction(this, entity, mouvement, entityUseMovementAction);
            Game.currentGame.PileAction(useEntityMouvementAction);
        }

        return canUseEntityMouvement;
    }

    public bool CanUseEntityMouvement(Entity entity, int mouvement){
        return entity.CanUseMovement(mouvement);
    }

    public bool TryToCreatePlayerUseEntityHeartsAction(Entity entity, HeartType[] hearts, out PlayerUseEntityHeartsAction useEntityHeartsAction){
        useEntityHeartsAction = new PlayerUseEntityHeartsAction(this, entity, hearts);
        var canUseEntityHearts =  CanUseEntityHearts(entity, hearts);
        if(canUseEntityHearts){
            entity.TryToCreateEntityPayHeartCostAction(hearts, out EntityPayHeartCostAction entityPayHeartCostAction);
            useEntityHeartsAction = new PlayerUseEntityHeartsAction(this, entity, hearts, entityPayHeartCostAction);
            Game.currentGame.PileAction(useEntityHeartsAction);
        }

        return canUseEntityHearts;
    }


    public bool CanUseEntityHearts(Entity entity, HeartType[] hearts){
        return entity.CanPayHeartCost(hearts);
    }



    private void SetupPermanentEffects(){

    }

    public override string ToString(){
        return $"Player {playerNum}";
    }
}
