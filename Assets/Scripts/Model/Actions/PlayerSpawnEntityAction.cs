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

    public Weapon weapon{
        get;
        protected set;
    }

    public int maxMovement{
        get;
        protected set;
    }

    public ScriptableEntity scriptableEntity{
        get;
        protected set;
    }

    public Entity entitySpawned{
        get;
        protected set;
    }

    public List<EntityEffect> permanentEffects{
        get;
        protected set;
    }

    public PlayerSpawnEntityAction(Player player, EntityModel model, string name, Tile startingTile, Health startingHealth, int startingMaxMovement, List<EntityEffect> permanentEffects, Direction startingDirection = Direction.North,  Weapon weapon = null, Action requiredAction = null) : base(player, requiredAction){
        tile = startingTile;
        this.model = model;
        this.name = name;
        health = startingHealth;
        this.weapon = weapon;
        maxMovement = startingMaxMovement;
        direction = startingDirection;
        this.permanentEffects = permanentEffects;
        entitySpawned = Entity.noEntity;
        scriptableEntity = null;
    }

    public PlayerSpawnEntityAction(Player player, ScriptableEntity scriptableEntity, Tile startingTile, Direction startingDirection = Direction.North, Action requiredAction = null) : base(player, requiredAction){
        tile = startingTile;
        model = scriptableEntity.entityModel;
        name = scriptableEntity.entityName;
        health = scriptableEntity.health;
        if(scriptableEntity.scriptableWeapon == null){
            weapon = Weapon.noWeapon;
        }
        else{
            weapon = new Weapon(scriptableEntity.scriptableWeapon);
        }
        direction = startingDirection;
        permanentEffects = new List<EntityEffect>();
        entitySpawned = Entity.noEntity;
        this.scriptableEntity = scriptableEntity;
    }

    protected override bool Perform(){
        if(scriptableEntity == null){
            entitySpawned = new Entity(player, model, name, tile, health, maxMovement, permanentEffects, direction, weapon);
        }
        else{
            entitySpawned = new Entity(player, scriptableEntity, tile, direction);
        }
        
        return player.TryToSpawnEntity(entitySpawned);
    }
}
