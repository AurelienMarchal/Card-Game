using System.Collections.Generic;
using UnityEngine;

public class Hero : Entity{

    public Hero(Player player, EntityModel model, string name, Tile startingTile, Health startingHealth, Damage starttingAtkDamage, int startingMaxMovement, List<EntityEffect> permanentEffects, Direction startingDirection = Direction.North, Weapon weapon = Weapon.noWeapon) : base(player, model, name, startingTile, startingHealth, startingMaxMovement, permanentEffects, startingDirection){
        movementLeft = maxMovement;
    }

    public Hero(Player player, ScriptableHero scriptableHero, Tile startingTile, Direction startingDirection = Direction.North, Weapon weapon = Weapon.noWeapon) : base(player, scriptableHero, startingTile, startingDirection){
        movementLeft = maxMovement;
    }

    public Weapon weapon{
        get;
        protected set;
    }

    public override bool TryToMove(Tile tile){

        var result = base.TryToMove(tile);

        return result;
        
    }

    public override bool TryToCreateEntityPayHeartCostAction(HeartType[] heartCost, out EntityPayHeartCostAction entityPayHeartCostAction, Action requiredAction = null){
        entityPayHeartCostAction = new EntityPayHeartCostAction(this, heartCost, requiredAction);
        var canPayHeartCost = CanPayHeartCost(heartCost);
        if(canPayHeartCost){
            Game.currentGame.PileAction(entityPayHeartCostAction);
        }

        return canPayHeartCost;
    }

    public override bool TryToPayHeartCost(HeartType[] heartCost){
        return health.TryToPayHeartCost(heartCost, false);
    }

    public override bool CanPayHeartCost(HeartType[] heartCost){
        var result = health.CanPayHeartCost(heartCost, out bool willDie);
        return result && !willDie;
    }

    protected override Cost CalculateCostToAtk(){
        return weapon.costToUse;
    }

    protected override int CalculateRange(){
        return weapon.range;
    }

    protected override Damage CalculateAtkDamage(){
        return weapon.atkDamage;
    }

    public override bool CanAttack(Entity entity){

        if(entity == this){
            return false;
        }

        if(weapon == Weapon.noWeapon){
            return false;
        }

        if(entity.currentTile.gridX != currentTile.gridX && entity.currentTile.gridY != currentTile.gridY){
            return false;
        }

        if(entity.currentTile.Distance(currentTile) > range){
            return false;
        }

        if(DirectionsExtensions.FromCoordinateDifference(
            entity.currentTile.gridX - currentTile.gridX,
            entity.currentTile.gridY - currentTile.gridY
            ) != direction){
                return false;
        }

        return true;
    }

    public override bool CanAttackByChangingDirection(Entity entity){

        if(entity == this){
            return false;
        }

        if(weapon == Weapon.noWeapon){
            return false;
        }

        if(entity.currentTile.gridX != currentTile.gridX && entity.currentTile.gridY != currentTile.gridY){
            return false;
        }

        if(entity.currentTile.Distance(currentTile) > weapon.range){
            return false;
        }

        if(!CanChangeDirection(DirectionsExtensions.FromCoordinateDifference(
            entity.currentTile.gridX - currentTile.gridX,
            entity.currentTile.gridY - currentTile.gridY
            ))){
                return false;
        }

        return true;
    }

    public override bool CanPayAtkCost(){
        if(weapon == null){
            return false;
        }

        return CanPayHeartCost(weapon.costToUse.heartCost) && CanUseMovement(weapon.costToUse.mouvementCost);
    }

    public override void GetTilesAndEntitiesAffectedByAtk(out Entity[] entitiesAffected, out Tile[] tilesAffected){
        if(weapon == Weapon.noWeapon){
            entitiesAffected = new Entity[0];
            tilesAffected = new Tile[0];
            return;
        }

        weapon.GetTilesAndEntitiesAffectedByAtk(currentTile, direction, out entitiesAffected, out tilesAffected);

    }





}

