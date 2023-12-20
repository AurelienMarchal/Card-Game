using UnityEngine;

public class MoveToChangeTileTypeEffect : Effect
{
    public Entity associatedEntity{
        get;
        protected set;
    }

    public TileType tileType{
        get;
        protected set;
    }

    public MoveToChangeTileTypeEffect(Entity entity, TileType tileType){
        associatedEntity = entity;
        this.tileType = tileType;
    }

    protected override void Activate(){
        Game.currentGame.PileAction(new TileChangeTypeAction(associatedEntity.currentTile, tileType, effectActivatedAction), false);
    }

    public override bool CanBeActivated()
    {
        return true;
    }

    public override bool Trigger(Action action)
    {
        switch(action){
            case EntityMoveAction entityMoveAction: 
                if(entityMoveAction.wasPerformed && entityMoveAction.entity == associatedEntity){
                    return true;
                }
                return false;

            default : return false;
        }
    }

    public override string GetEffectText(){
        return $"Every time this entity moves, the tile under it is transformed into a{tileType.ToTileString()}";
    }

    
}
