
using System.Collections.Generic;
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

    public Damage atkDamage{
        get;
        protected set;
    }

    public bool hasAttacked{
        get;
        set;
    }

    public const Entity noEntity = null;

    public List<Effect> effects{
        get;
        protected set;
    }

    public Entity(Player player, EntityModel model, string name, Tile startingTile, Health startingHealth, Damage startingAtkDamage, Direction startingDirection = Direction.North){
        this.model = model;
        this.name = name;
        currentTile = startingTile;
        health = startingHealth;
        atkDamage = startingAtkDamage;
        direction = startingDirection;
        this.player = player;
        effects = new List<Effect>();
        SetupPermanentEffects();
    }

    public virtual bool TryToCreateEntityMoveAction(Tile tile, Action requiredAction, out EntityMoveAction entityMoveAction){
        entityMoveAction = new EntityMoveAction(this, currentTile, tile, requiredAction);
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

        if(Game.currentGame.board.GetEntityAtTile(tile) != Entity.noEntity){
            return false;
        }

        //Check move condition
        if(tile.gridX != currentTile.gridX && tile.gridY != currentTile.gridY){
            return false;
        }

        var distance = tile.Distance(currentTile);

        if(distance > 1){
            return false;
        }

        return true;
    }

    protected virtual void Move(Tile tile){
        direction = DirectionsExtensions.FromCoordinateDifference(tile.gridX - currentTile.gridX, tile.gridY - currentTile.gridY);
        currentTile = tile;
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
        entityAttackAction = new EntityAttackAction(this, entity, requiredAction);
        var canAttack = CanAttack(entity);
        if(canAttack){
            Game.currentGame.PileAction(entityAttackAction);
        }

        return canAttack;
    }

    public bool CanAttack(Entity entity){
        if(hasAttacked){
            return false;
        }

        if(entity.currentTile.Distance(currentTile) > 1){
            return false;
        }

        return true;
    }


    public bool TakeDamage(Damage damage){
        Debug.Log($"{this} taking {damage} damage");
        var isDead = health.TakeDamage(damage);
        Debug.Log($"{this} current {health}");
        return isDead;
    }

    protected void SetupPermanentEffects(){

    }
}
