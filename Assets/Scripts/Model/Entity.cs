
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity
{
    public EntityModel model{
        get;
        protected set;
    }

    public string name{
        get;
        protected set;
    }

    public Tile currentTile{
        get;
        protected set;
    }

    public Health health{
        get;
        protected set;
    }

    public Direction direction{
        get;
        protected set;
    }

    public Player player{
        get;
        protected set;
    }

    public Weapon weapon{
        get;
        protected set;
    }

    public int movementLeft{
        get;
        protected set;
    }

    public int maxMovement{
        get;
        protected set;
    }

    public Cost costToMove{
        get{
            var weightedDownBuffCount = NumberOfBuffs<WeightedDownBuff>();
            return new Cost(1 + weightedDownBuffCount);
        }
    }

    public const Entity noEntity = null;
    public const int maxMovementCap = 10;

    public List<EntityEffect> effects{
        get;
        protected set;
    }

    public List<EntityBuff> buffs{
        get;
        private set;
    }

    public Entity(Player player, EntityModel model, string name, Tile startingTile, Health startingHealth, int startingMaxMovement, List<EntityEffect> permanentEffects, Direction startingDirection = Direction.North, Weapon weapon = Weapon.noWeapon){
        this.player = player;
        this.model = model;
        this.name = name;
        currentTile = startingTile;
        health = startingHealth.Clone() as Health;
        this.weapon = weapon;
        maxMovement = startingMaxMovement;
        movementLeft = 0;
        direction = startingDirection;
        effects = new List<EntityEffect>();
        buffs = new List<EntityBuff>(); 
        AddEffectList(permanentEffects);
        AddDefaultPermanentEffects();
    }

    public Entity(Player player, ScriptableEntity scriptableEntity, Tile startingTile, Direction startingDirection = Direction.North){
        this.player = player;
        model = scriptableEntity.entityModel;
        name = scriptableEntity.entityName;
        currentTile = startingTile;
        health = scriptableEntity.health.Clone() as Health;
        //Debug.Log($"Cloning health successfull : {health != scriptableEntity.health}");
        if(scriptableEntity.scriptableWeapon == null){
            weapon = Weapon.noWeapon;
        }
        else{
            weapon = new Weapon(scriptableEntity.scriptableWeapon);
        }
        maxMovement = scriptableEntity.maxMovement;
        movementLeft = 0;
        direction = startingDirection;
        effects = new List<EntityEffect>();
        buffs = new List<EntityBuff>(); 
        AddEffectList(scriptableEntity.scriptableEffects);
        AddDefaultPermanentEffects();
    }

    public virtual bool TryToCreateEntityMoveAction(Tile tile, Action requiredAction, out EntityMoveAction entityMoveAction){
        var newdirection = DirectionsExtensions.FromCoordinateDifference(tile.gridX - currentTile.gridX, tile.gridY - currentTile.gridY);
        TryToCreateEntityChangeDirectionAction(newdirection, requiredAction, out EntityChangeDirectionAction entityChangeDirectionAction);
        entityMoveAction = new EntityMoveAction(this, currentTile, tile, entityChangeDirectionAction);
        var result = CanMove(tile);
        if(result){
            Game.currentGame.PileAction(entityMoveAction);
        }

        return result;
    }

    public virtual bool TryToMove(Tile tile){

        var result = CanMove(tile);
        if(result){
            Move(tile);
        }

        return result;

    }

    public virtual bool CanMove(Tile tile){
        if(tile == currentTile){
            return false;
        }

        if(NumberOfBuffs<EntityCannotMoveBuff>() > 0){
            return false;
        }

        if(Game.currentGame.board.GetEntityAtTile(tile) != Entity.noEntity){
            return false;
        }

        //Check move condition
        if(tile.gridX != currentTile.gridX && tile.gridY != currentTile.gridY){
            return false;
        }

        if(DirectionsExtensions.FromCoordinateDifference(tile.gridX - currentTile.gridX, tile.gridY - currentTile.gridY) != direction){
            return false;
        }

        var distance = tile.Distance(currentTile);

        if(distance > 1){
            return false;
        }

        return true;
    }

    public virtual bool CanMoveByChangingDirection(Tile tile){
        if(tile == currentTile){
            return false;
        }

        if(Game.currentGame.board.GetEntityAtTile(tile) != Entity.noEntity){
            return false;
        }

        //Check move condition
        if(tile.gridX != currentTile.gridX && tile.gridY != currentTile.gridY){
            return false;
        }

        if(!CanChangeDirection(DirectionsExtensions.FromCoordinateDifference(tile.gridX - currentTile.gridX, tile.gridY - currentTile.gridY))){
            return false;
        }

        var distance = tile.Distance(currentTile);

        if(distance > 1){
            return false;
        }

        return true;
    }

    protected virtual void Move(Tile tile){
        currentTile = tile;
    }

    public virtual bool TryToCreateEntityChangeDirectionAction(Direction newDirection, Action requiredAction, out EntityChangeDirectionAction entityChangeDirectionAction){
        entityChangeDirectionAction = new EntityChangeDirectionAction(this, newDirection, requiredAction);
        var result = CanChangeDirection(newDirection);
        if(result){
            Game.currentGame.PileAction(entityChangeDirectionAction);
        }

        return result;
    }

    public virtual bool TryToChangeDirection(Direction newDirection){

        var result = CanChangeDirection(newDirection);
        if(result){
            ChangeDirection(newDirection);
        }

        return result;

    }

    public virtual bool CanChangeDirection(Direction newDirection){
        return true;
    }

    protected virtual void ChangeDirection(Direction newDirection){
        direction = newDirection;
    }

    public virtual bool TryToTeleport(Tile tile){

        var result = CanTeleport(tile);
        if(result){
            Teleport(tile);
        }

        return result;

    }

    public virtual bool CanTeleport(Tile tile){
        if(tile == currentTile){
            return false;
        }

        if(Game.currentGame.board.GetEntityAtTile(tile) != Entity.noEntity){
            return false;
        }

        return true;
    }

    protected virtual void Teleport(Tile tile){
        currentTile = tile;
    }

    public bool TryToCreateEntityAttackAction(Entity entity,  out EntityAttackAction entityAttackAction, Action requiredAction = null){
        var newdirection = DirectionsExtensions.FromCoordinateDifference(entity.currentTile.gridX - currentTile.gridX, entity.currentTile.gridY - currentTile.gridY);
        TryToCreateEntityChangeDirectionAction(newdirection, requiredAction, out EntityChangeDirectionAction entityChangeDirectionAction);
        entityAttackAction = new EntityAttackAction(this, entity, requiredAction:entityChangeDirectionAction);
        var canAttack = CanAttack(entity);
        if(canAttack){
            Game.currentGame.PileAction(entityAttackAction);
        }

        return canAttack;
    }

    public bool CanAttack(Entity entity){

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

        if(DirectionsExtensions.FromCoordinateDifference(
            entity.currentTile.gridX - currentTile.gridX,
            entity.currentTile.gridY - currentTile.gridY
            ) != direction){
                return false;
        }

        return true;
    }

    public bool CanAttackByChangingDirection(Entity entity){

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

    public bool CanPayWeaponCost(){
        if(weapon == null){
            return false;
        }

        return CanPayHeartCost(weapon.costToUse.heartCost) && CanUseMovement(weapon.costToUse.mouvementCost);
    }

    public void GetTilesAndEntitiesAffectedByAtk(out Entity[] entitiesAffected, out Tile[] tilesAffected){
        if(weapon == Weapon.noWeapon){
            entitiesAffected = new Entity[0];
            tilesAffected = new Tile[0];
            return;
        }

        weapon.GetTilesAndEntitiesAffectedByAtk(currentTile, direction, out entitiesAffected, out tilesAffected);

    }


    public bool TakeDamage(Damage damage){
        Debug.Log($"{this} taking {damage} damage");
        var isDead = health.TakeDamage(damage);
        Debug.Log($"{this} current {health}");
        return isDead;
    }



    public bool TryToCreateEntityUseMovementAction(int movement,  out EntityUseMovementAction entityUseMovementAction, Action requiredAction = null){
        entityUseMovementAction = new EntityUseMovementAction(movement, this, requiredAction);
        var canUseMovement = CanUseMovement(movement);
        if(canUseMovement){
            Game.currentGame.PileAction(entityUseMovementAction);
        }

        return canUseMovement;
    }

    public bool TryToUseMovement(int movement){
        var canUseMovement = CanUseMovement(movement);
        if(canUseMovement){
            UseMovement(movement);
        }
        return canUseMovement;
    }

    public bool CanUseMovement(int movement){
        return movement <= movementLeft && movement >= 0;
    }

    private void UseMovement(int movement){
        movementLeft = Math.Clamp(movementLeft - movement, 0, maxMovement);
    }

    public virtual bool TryToCreateEntityPayHeartCostAction(HeartType[] heartCost, out EntityPayHeartCostAction entityPayHeartCostAction, Action requiredAction = null){
        entityPayHeartCostAction = new EntityPayHeartCostAction(this, heartCost, requiredAction);
        var canPayHeartCost = CanPayHeartCost(heartCost);
        if(canPayHeartCost){
            Game.currentGame.PileAction(entityPayHeartCostAction);
        }

        return canPayHeartCost;
    }

    public virtual bool TryToPayHeartCost(HeartType[] heartCost){
        return health.TryToPayHeartCost(heartCost, true);
    }

    public virtual bool CanPayHeartCost(HeartType[] heartCost){
        return health.CanPayHeartCost(heartCost, out _);
    }

    public bool TryToIncreaseMaxMovement(){
        var canIncreaseMaxMovement = CanIncreaseMaxMovement();
        if(canIncreaseMaxMovement){
            IncreaseMaxMovement();
        }
        return canIncreaseMaxMovement;
    }

    public bool CanIncreaseMaxMovement(){
        return maxMovement < maxMovementCap;
    }

    private void IncreaseMaxMovement(){
        maxMovement = Math.Clamp(maxMovement + 1, 0, maxMovementCap);
    }

    public void ResetMovement(){
        movementLeft = maxMovement;
    }

    public void AddEffectList(List<ScriptableEffect> scriptableEffects){
        foreach (var scriptableEffect in scriptableEffects){
            AddEffect(scriptableEffect);
        }
    }

    public void AddEffect(ScriptableEffect scriptableEffect){
        if (scriptableEffect.GetEffect() is EntityEffect entityEffect)
        {
            AddEffect(entityEffect);
        }
    }

    public void AddEffect(EntityEffect entityEffect){
        entityEffect.associatedEntity = this;
        effects.Add(entityEffect);
        UpdateBuffsAccordingToEffects();
    }

    public void RemoveEffect(EntityEffect entityEffect){
        if(effects.Contains(entityEffect)){
            effects.Remove(entityEffect);
            UpdateBuffsAccordingToEffects();
        }
    }

    public void AddEffectList(List<EntityEffect> entityEffects){
        foreach (var effect in entityEffects){
            AddEffect(effect);
        }
    }

    protected void AddDefaultPermanentEffects(){
        effects.Add(new EntityDiesWhenHealthIsEmpty(this));
        effects.Add(new EntityIsWeightedDownByStoneHeartEffect(this));
    }

    public override string ToString()
    {
        return $"Entity {name}";
    }

    public void UpdateBuffsAccordingToEffects(){

        buffs.Clear();

        foreach(var effect in effects){
            foreach (var buff in effect.entityBuffs){
                buffs.Add(buff);
            }
        }

        Debug.Log($"{this} buffs : [{String.Join(", ", buffs)}]");
    }

    private int NumberOfBuffs<B>() where B : EntityBuff{
        var count = 0;
        foreach(var buff in buffs){
            switch(buff){
                case B _:
                    count += 1; break;
            }
        }

        return count;
    }
}
