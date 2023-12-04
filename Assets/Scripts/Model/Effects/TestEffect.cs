using UnityEngine;

public class TestEffect : Effect
{
    private Entity associatedEntity;

    private TileType tileType;

    public TestEffect(Entity entity, TileType tileType) : base(entity.player){
        associatedEntity = entity;
        this.tileType = tileType;
    }

    public override void OnEntityMoving(Entity entity){
        if(entity == associatedEntity){
            entity.currentTile.tileType = tileType;
        }
    }
}
