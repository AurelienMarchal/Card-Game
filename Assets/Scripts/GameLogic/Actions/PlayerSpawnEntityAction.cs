using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic{

    using GameEffect;
    using GameLogic.GameState;

    namespace GameAction{

        public class PlayerSpawnEntityAction : PlayerAction
        {
            
            public Entity entity{
                get;
                protected set;
            }

            public Tile tile{
                get;
                protected set;
            }

            public PlayerSpawnEntityAction(Player player, Entity entity, Tile startingTile, Action requiredAction = null) : base(player, requiredAction)
            {
                this.entity = entity;
                tile = startingTile;
            }

            protected override bool Perform(){

                entity.TryToSetStartingTile(tile);
                return player.TryToSpawnEntity(entity);
            }

            public override ActionState ToActionState()
            {
                var actionState = new PlayerSpawnEntityActionState();
                actionState.playerNum = player.playerNum;
                actionState.entitySpawned = entity.ToEntityState();
                actionState.tileNum = tile.num;
                return actionState;
            }
        }
        }
}