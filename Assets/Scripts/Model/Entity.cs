
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

    public int atk{
        get;
        protected set;
    }

    public bool hasAttacked{
        get;
        protected set;
    }

    public const Entity noEntity = null;

    public List<Effect> effects;

    public Entity(EntityModel model, string name, Tile startingTile, Health startingHealth, Player player, int startingAtk = 0, Direction startingDirection = Direction.North){
        this.model = model;
        this.name = name;
        currentTile = startingTile;
        health = startingHealth;
        atk = startingAtk;
        direction = startingDirection;
        this.player = player;
        effects = new List<Effect>();
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


    public virtual void OnStartGame(){
        
    }

    public virtual void OnStartTurn(){
    }

    public virtual void OnStartPlayerTurn(){
        hasAttacked = false;
    }

    public virtual void OnEndTurn(){
        
    }

    public virtual void OnEndPlayerTurn(){
        
    }

    public override string ToString()
    {
        return $"Entity {name} at tile {currentTile}";
    }

    public bool TakeDamage(Damage damage){
        Debug.Log($"{this} taking {damage} damage");
        var isDead = health.TakeDamage(damage);
        Debug.Log($"{this} current {health}");
        return isDead;
    }
}
