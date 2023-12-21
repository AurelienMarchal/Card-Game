
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

    public List<EntityEffect> effects{
        get;
        protected set;
    }

    public Entity(Player player, EntityModel model, string name, Tile startingTile, Health startingHealth, Damage startingAtkDamage, List<EntityEffect> permanentEffects, Direction startingDirection = Direction.North){
        this.player = player;
        this.model = model;
        this.name = name;
        currentTile = startingTile;
        health = startingHealth;
        atkDamage = startingAtkDamage;
        direction = startingDirection;
        effects = new List<EntityEffect>();
        AddEffectList(permanentEffects);
        AddDefaultPermanentEffects();
    }

    public Entity(Player player, ScriptableEntity scriptableEntity, Tile startingTile, Direction startingDirection = Direction.North){
        this.player = player;
        model = scriptableEntity.entityModel;
        name = scriptableEntity.entityName;
        currentTile = startingTile;
        health = scriptableEntity.health;
        atkDamage = scriptableEntity.atkDamage;
        direction = startingDirection;
        effects = new List<EntityEffect>();
        AddEffectList(scriptableEntity.scriptableEffects);
        AddDefaultPermanentEffects();
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

        if(entity == this){
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

    public void AddEffectList(List<ScriptableEffect> scriptableEffects){
        foreach (var scriptableEffect in scriptableEffects){
            AddEffect(scriptableEffect);
        }
    }

    public void AddEffect(ScriptableEffect scriptableEffect){
        var entityEffect = scriptableEffect.GetEffect() as EntityEffect;
        if(entityEffect != null){
            AddEffect(entityEffect);
        }
    }

    public void AddEffect(EntityEffect entityEffect){
        entityEffect.associatedEntity = this;
        effects.Add(entityEffect);
    }

    public void AddEffectList(List<EntityEffect> entityEffects){
        foreach (var effect in entityEffects){
            AddEffect(effect);
        }
    }

    protected void AddDefaultPermanentEffects(){

    }

    public override string ToString()
    {
        return $"Entity {name}";
    }
}
