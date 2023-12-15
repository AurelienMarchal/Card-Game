using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnEntityAction : PlayerAction
{

    public Tile tile{
        get;
        protected set;
    }

    public EntityModel model{
        get;
        protected set;
    }

    public string name{
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

    public int atk{
        get;
        protected set;
    }

    public Entity entitySpawned{
        get;
        protected set;
    }

    public PlayerSpawnEntityAction(Player player, EntityModel model, string name, Tile startingTile, Health startingHealth, int starttingAtk = 0, Direction startingDirection = Direction.North,  Action requiredAction = null) : base(player, requiredAction){
        tile = startingTile;
        this.model = model;
        this.name = name;
        health = startingHealth;
        atk = starttingAtk;
        direction = startingDirection;
        entitySpawned = Entity.noEntity;
    }

    protected override bool Perform(){
        entitySpawned = new Entity(model, name, tile, health, player, atk, direction);
        return player.TryToSpawnEntity(entitySpawned);
    }
}
