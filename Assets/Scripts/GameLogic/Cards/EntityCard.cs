using System.Collections;
using System.Collections.Generic;

namespace GameLogic
{
    using System;
    using GameAction;
    using GameEffect;
    using GameState;

    public class EntityCard : Card
    {

        public Entity entity
        {
            get;
            private set;
        }

        public EntityCard(uint num, Player player, Cost cost, Entity entity) : base(num, player, cost, needsEntityTarget: false, needsTileTarget: true)
        {
            this.entity = entity;
        }

        public override bool CanBeActivated(Tile targetTile = null, Entity targetEntity = null)
        {
            if (targetTile == null)
            {
                return false;
            }

            if (Game.currentGame.board.GetEntityAtTile(targetTile) != Entity.noEntity)
            {
                return false;
            }

            if (!PossibleTileTargets().Contains(targetTile))
            {
                return false;
            }

            return true;
        }

        protected override bool Activate(Tile targetTile = null, Entity targetEntity = null)
        {
            //Marche pas si l'entity deja mis sur le board 
            //l'entite spawné doit etre une copie de l'entite dans la carte
            //l'entite dans ka carte ne doit pas avoir de num (ni de currentTilegr) 
            return player.TryToCreateSpawnEntityAction(entity, targetTile, out _);
        }

        public override List<Tile> PossibleTileTargets()
        {
            //TODO : check if entity already on tile
            var toReturn = Game.currentGame.board.GetTileSquareAroundTile(entity.player.hero.currentTile, 5);
            toReturn.Remove(entity.player.hero.currentTile);
            return toReturn;
        }

        public override string GetText()
        {
            return "Spawn " + entity.name;
        }

        public override string GetCardName()
        {
            return entity.name;
        }
    }
}