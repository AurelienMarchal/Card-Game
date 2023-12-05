using UnityEngine;

public class MoveToChangeTileTypeEffect : Effect
{
    private Entity associatedEntity;

    private TileType tileType;

    public MoveToChangeTileTypeEffect(Entity entity, TileType tileType) : base(entity.player){
        associatedEntity = entity;
        this.tileType = tileType;
    }

    public override void Activate(bool depile){
        base.Activate(depile);
        Game.currentGame.PileAction(new TileChangeTypeAction(associatedEntity.currentTile, tileType), depile);
    }

    public override bool CanBeActivated()
    {
        return true;
    }

    public override bool Trigger(Action action)
    {
        switch(action){
            case EntityMoveAction entityMoveAction: 
                if(entityMoveAction.wasPerfomed && entityMoveAction.entity == associatedEntity){
                    return true;
                }
                return false;

            default : return false;
        }
    }

    
}
