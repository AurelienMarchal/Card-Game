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

    public PlayerSpawnEntityAction(Player player, EntityModel model, string name, Tile startingTile, Health startingHealth, Damage startingDamageAtk, List<EntityEffect> permanentEffects, Direction startingDirection = Direction.North, Action requiredAction = null) : base(player, requiredAction){
        tile = startingTile;
        this.model = model;
        this.name = name;
        health = startingHealth;
        damageAtk = startingDamageAtk;
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
        damageAtk = scriptableEntity.atkDamage;
        direction = startingDirection;
        permanentEffects = new List<EntityEffect>();
        entitySpawned = Entity.noEntity;
    }

    protected override bool Perform(){
        if(scriptableEntity == null){
            entitySpawned = new Entity(player, model, name, tile, health, damageAtk, permanentEffects, direction);
        }
        else{
            entitySpawned = new Entity(player, scriptableEntity, tile, direction);
        }
        
        return player.TryToSpawnEntity(entitySpawned);
    }
}
