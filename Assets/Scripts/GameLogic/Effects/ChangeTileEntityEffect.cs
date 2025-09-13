using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameLogic{

    using GameAction;

    namespace GameEffect{
        public class ChangeTileEntityEffect : EntityEffect{

            public TileType tileType{
                get;
                protected set;
            }

            public ChangeTileEntityEffect(TileType tileType, Entity entity) : base(entity){
                this.tileType = tileType;
            }

            protected override void Activate()
            {
                Game.currentGame.PileAction(new TileChangeTypeAction(associatedEntity.currentTile, tileType, effectActivatedAction));
            }

            public override string GetEffectText()
            {
                return $"Change tile under {associatedEntity} to {tileType.ToTileString()}";
            }

            public override void GetTilesAndEntitiesAffected(out Entity[] entitiesAffected, out Tile[] tilesAffected)
            {
                entitiesAffected = new Entity[0];
                tilesAffected = new Tile[]{associatedEntity.currentTile};
            }
        }
    }
}
