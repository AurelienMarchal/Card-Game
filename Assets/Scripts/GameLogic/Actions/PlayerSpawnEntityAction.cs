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

            public PlayerSpawnEntityAction(Player player, EntityModel model, string name, Tile startingTile, Health startingHealth, int startingMaxMovement, List<EntityEffect> permanentEffects, Direction startingDirection = Direction.North, Weapon weapon = null, Action requiredAction = null) : base(player, requiredAction)
            {

                entity = new Entity(player, model, name, startingTile, startingHealth, startingMaxMovement, permanentEffects, startingDirection);
                tile = startingTile;
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
                throw new System.NotImplementedException();
            }
        }
        }
}