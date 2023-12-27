using UnityEngine;

public class ThrowProjectileEffect : EntityEffect
{
    public Direction direction{
        get;
        protected set;
    }

    public Damage damage{
        get;
        protected set;
    }

    public int range{
        get;
        protected set;
    }
    
    public Entity entityHit{
        get;
        protected set;
    }

    public Tile tileReached{
        get;
        protected set;
    }

    
    public ThrowProjectileEffect(Entity casterEntity, Direction direction, Damage damage, int range) : base(casterEntity){
        this.direction = direction;
        this.damage = damage;
        this.range = range;
        entityHit = Entity.noEntity;
        tileReached = casterEntity.currentTile;
    }

    protected override void Activate(){
        var tileChecked = Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, direction);
        var lastTileChecked = tileChecked;
        var entityFound = Game.currentGame.board.GetEntityAtTile(tileChecked);

        var counter = Game.currentGame.board.gridHeight * Game.currentGame.board.gridWidth;
        while(counter > 0){
            counter --;
            //Debug.Log($"counter {counter}, entityFound {entityFound}, tileChecked {tileChecked}");
            if(entityFound != Entity.noEntity){
                break;
            }
            if(tileChecked == Tile.noTile){
                break;
            }

            if(tileChecked.Distance(associatedEntity.currentTile) > range){
                break;
            }

            lastTileChecked = tileChecked;
            tileChecked = Game.currentGame.board.NextTileInDirection(tileChecked, direction);
            entityFound = Game.currentGame.board.GetEntityAtTile(tileChecked);
        }

        if(entityFound != Entity.noEntity){
            entityHit = entityFound;
            tileReached = entityHit.currentTile;
        }

        else if(tileChecked == Tile.noTile){
            entityHit = Entity.noEntity;
            tileReached = lastTileChecked;
        }

        else if(tileChecked != Tile.noTile){
            entityHit = Entity.noEntity;
            tileReached = tileChecked;
        }

        if(entityHit != Entity.noEntity){
            Game.currentGame.PileAction(new EntityTakeDamageAction(entityHit, damage, effectActivatedAction));
        }
    }

    public override bool CanBeActivated(){
        return Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, associatedEntity.direction) != Tile.noTile;
    }
}