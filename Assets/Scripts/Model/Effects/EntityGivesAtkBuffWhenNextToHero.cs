using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO
public class EntityGivesAtkBuffWhenNextToHero : EntityEffect
{
    public int amount{
        get;
        private set;
    }

    public EntityGivesAtkBuffWhenNextToHero(int amount, Entity entity, bool displayOnUI = true) : base(entity, displayOnUI){
        this.amount = amount;
    }

    public override bool Trigger(Action action){
        switch (action){
            case StartGameAction startGameAction:
                return startGameAction.wasPerformed;

            case PlayerSpawnEntityAction playerSpawnEntityAction:
                return playerSpawnEntityAction.entitySpawned == associatedEntity && playerSpawnEntityAction.wasPerformed;

            case EntityMoveAction entityMoveAction:
                return entityMoveAction.wasPerformed;

            case EntityDieAction entityDieAction:
                return entityDieAction.wasPerformed;
        }

        return false;
    }

    protected override void Activate(){
        entityBuffs.Clear();

        var entityNorth = Game.currentGame.board.GetEntityAtTile(Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.North));

        var entitySouth = Game.currentGame.board.GetEntityAtTile(Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.South));

        var entityEast = Game.currentGame.board.GetEntityAtTile(Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.East));

        var entityWest = Game.currentGame.board.GetEntityAtTile(Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, Direction.West));

        if(entityNorth != Entity.noEntity){

        }

        //entityBuffs.Add(new AtkBuff(amount));
    }


}
