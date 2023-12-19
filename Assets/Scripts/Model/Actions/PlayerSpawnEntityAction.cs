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

    public Damage damageAtk{
        get;
        protected set;
    }

    public Entity entitySpawned{
        get;
        protected set;
    }

    public PlayerSpawnEntityAction(Player player, EntityModel model, string name, Tile startingTile, Health startingHealth, Damage startingDamageAtk, Direction startingDirection = Direction.North,  Action requiredAction = null) : base(player, requiredAction){
        tile = startingTile;
        this.model = model;
        this.name = name;
        health = startingHealth;
        damageAtk = startingDamageAtk;
        direction = startingDirection;
        entitySpawned = Entity.noEntity;
    }

    protected override bool Perform(){
        entitySpawned = new Entity(player, model, name, tile, health, damageAtk, direction);
        return player.TryToSpawnEntity(entitySpawned);
    }
}
