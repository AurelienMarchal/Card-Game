using System.Collections.Generic;
using UnityEngine;


public class MinionCard : Card
{
    public EntityModel entityModel{
        get;
        protected set;
    }

    public Health minionHealth{
        get;
        protected set;
    }

    public Damage atkDamage{
        get;
        protected set;
    }

    public int maxMovement{
        get;
        protected set;
    }

    public string entityName{
        get;
        protected set;
    }

    public List<EntityEffect> permanentEffects{
        get;
        protected set;
    }

    public ScriptableEntity scriptableEntity{
        get;
        protected set;
    }

    public MinionCard(Player player, Cost cost, EntityModel entityModel, Health minionHealth, Damage atkDamage, int maxMovement, List<EntityEffect> permanentEffects, string cardName, string text, bool needsTileTarget = false, bool needsEntityTarget = false) : base(player, cost, cardName, text, needsTileTarget, needsEntityTarget){
        this.entityModel = entityModel;
        this.minionHealth = minionHealth;
        this.atkDamage = atkDamage;
        this.maxMovement = maxMovement;
        entityName = cardName;
        this.permanentEffects = permanentEffects;
        scriptableEntity = null;
    }


    public MinionCard(Player player, ScriptableMinionCard scriptableMinionCard) : base(player, scriptableMinionCard){
        entityModel = scriptableMinionCard.scriptableEntity.entityModel;
        minionHealth = scriptableMinionCard.scriptableEntity.health;
        atkDamage = scriptableMinionCard.scriptableEntity.atkDamage;
        maxMovement = scriptableMinionCard.scriptableEntity.maxMovement;
        entityName = scriptableMinionCard.scriptableEntity.entityName;
        permanentEffects = new List<EntityEffect>();
        scriptableEntity = scriptableEntity;
    }

    protected override bool Activate(Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity)
    {
        if(targetTile == Tile.noTile){
            var spawnTile = Game.currentGame.board.NextTileInDirection(player.hero.currentTile, player.hero.direction);
            if(scriptableEntity == null){
                return player.TryToCreateSpawnEntityAction(entityModel, cardName, spawnTile, minionHealth, atkDamage, maxMovement, player.hero.direction, permanentEffects, cardPlayedAction, out _);
            }
            else{
                return player.TryToCreateSpawnEntityAction(scriptableEntity, spawnTile, player.hero.direction, cardPlayedAction, out _);
            }
        }
        
        return player.TryToCreateSpawnEntityAction(entityModel, entityName, targetTile, minionHealth, atkDamage, maxMovement, player.hero.direction, permanentEffects, cardPlayedAction, out _);
    }

    public override bool CanBeActivated(Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity)
    {

        if(targetTile == Tile.noTile){
            var spawnTile = Game.currentGame.board.NextTileInDirection(player.hero.currentTile, player.hero.direction);
            Debug.Log($"{this} can be activated : {player.CanSpawnEntityAt(spawnTile)}");
            return player.CanSpawnEntityAt(spawnTile);

        }
        Debug.Log($"{this} can be activated : {player.CanSpawnEntityAt(targetTile)}");
        return player.CanSpawnEntityAt(targetTile);
    }
}