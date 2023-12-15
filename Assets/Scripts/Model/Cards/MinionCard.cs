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

    public MinionCard(Player player, Cost cost, EntityModel entityModel, Health minionHealth, string cardName, string text, bool needsTileTarget = false, bool needsEntityTarget = false) : base(player, cost, cardName, text, needsTileTarget, needsEntityTarget){
        this.entityModel = entityModel;
        this.minionHealth = minionHealth;
    }


    public MinionCard(Player player, ScriptableMinionCard scriptableMinionCard) : base(player, scriptableMinionCard){
        minionHealth = scriptableMinionCard.minionHealth;
        entityModel = scriptableMinionCard.entityModel;
    }

    protected override bool Activate(Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity)
    {
        if(targetTile == Tile.noTile){
            var spawnTile = Game.currentGame.board.NextTileInDirection(player.hero.currentTile, player.hero.direction);
            return player.TryToCreateSpawnEntityAction(entityModel, cardName, spawnTile, minionHealth, 0, player.hero.direction, cardPlayedAction, out _);

        }
        
        return player.TryToCreateSpawnEntityAction(entityModel, cardName, targetTile, minionHealth, 0, player.hero.direction, cardPlayedAction, out _);
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