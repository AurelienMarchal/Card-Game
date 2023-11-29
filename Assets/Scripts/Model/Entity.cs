
using UnityEngine;

public class Entity
{
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

    public Entity(string name, Tile startingTile, Health startingHealth, Player player, int starttingAtk = 0, Direction startingDirection = Direction.North){
        this.name = name;
        currentTile = startingTile;
        health = startingHealth;
        atk = starttingAtk;
        direction = startingDirection;
        this.player = player;
    }

    public virtual bool TryToMove(Tile tile){
        if(tile == currentTile){
            return false;
        }

        //Check move condition
        if(tile.gridX != currentTile.gridX && tile.gridY != currentTile.gridY){
            return false;
        }

        var distance = tile.Distance(currentTile);

        if(!player.TryToUseMovement(distance)){
            return false;
        }
        
        direction = DirectionsExtensions.FromCoordinateDifference(tile.gridX - currentTile.gridX, tile.gridY - currentTile.gridY);
        currentTile = tile;

        
        return true;
    }


    public virtual bool TryToAttack(Entity entity){
        var tile = entity.currentTile;
        if(tile == currentTile){
            return false;
        }

        //Check move condition
        if(tile.gridX != currentTile.gridX && tile.gridY != currentTile.gridY){
            return false;
        }

        var distance = tile.Distance(currentTile);
        if(distance > 1){
            Debug.Log($"{this} is too far away from {entity} to attack");
            return false;
        }

        if(hasAttacked){
            Debug.Log($"{this} has already attacked");
            return false;
        }

        if(!player.TryToUseMovement(atk)){
            return false;
        }

        entity.TakeDamage(atk);
        hasAttacked = true;
        
        direction = DirectionsExtensions.FromCoordinateDifference(tile.gridX - currentTile.gridX, tile.gridY - currentTile.gridY);
        
        return true;
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

    public bool TakeDamage(int damage){
        Debug.Log($"{this} taking {damage} damage");
        var isDead = health.TakeDamage(damage);
        Debug.Log($"{this} current {health}");
        return isDead;
    }
}
