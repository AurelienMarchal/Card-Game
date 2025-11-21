using System.Collections;
using System.Collections.Generic;

namespace GameLogic
{

    using GameEffect;
    using GameAction;

    public class SimpleDamagingEffect : EntityEffect, DealsDamageInterface, HasCostInterface, HasRangeInterface, CanBeActivatedInterface
    {

        private Cost cost
        {
            get; set;
        }

        private Damage damage
        {
            get; set;
        }

        private int range
        {
            get; set;
        }

        List<Tile> tilesInRange;

        List<Entity> entitiesInRange;

        public SimpleDamagingEffect(Entity entity, Cost cost, Damage damage, int range, bool displayOnUI = true) : base(entity, displayOnUI)
        {
            this.cost = cost;
            this.damage = damage;
            this.range = range;
            tilesInRange = new List<Tile>();
            entitiesInRange = new List<Entity>();
        }

        void CanBeActivatedInterface.Activate()
        {
            if (entitiesInRange.Count > 0 && entitiesInRange[0] != Entity.noEntity)
            {
                var entityHit = entitiesInRange[0];
                var tileReached = entityHit.currentTile;
                Game.currentGame.PileAction(new EntityTakeDamageAction(entityHit, damage));
            }
        }

        public bool CanBeActivated()
        {
            return false;
        }

        public override string GetEffectText()
        {
            return $"Deal {damage} with range {range}";
        }

        public override string GetEffectName()
        {
            return $"Simple damaging effect";
        }

        public bool CheckTriggerToActivate(Action action)
        {
            return false;
        }

        public bool CheckTriggerToUpdateTilesAffected(Action action)
        {
            switch (action)
            {
                case PlayerSpawnEntityAction playerSpawnEntityAction:
                    return playerSpawnEntityAction.wasPerformed;
                case EntityMoveAction entityMoveAction:
                    return entityMoveAction.wasPerformed;
                case EntityDieAction entityDieAction:
                    return entityDieAction.wasPerformed;
            }

            return false;
        }
        
        public override bool CheckTriggerToUpdateEntitiesAffected(Action action)
        {
            return false;
        }

        public bool CheckTriggerToUpdateCost(Action action)
        {
            return false;
        }

        public bool CheckTriggerToUpdateDamage(Action action)
        {
            return false;
        }

        public bool CheckTriggerToUpdateRange(Action action)
        {
            return false;
        }

        public Cost GetCost()
        {
            return cost;
        }

        public Damage GetDamage()
        {
            return damage;
        }

        public int GetRange()
        {
            return range;
        }

        public List<Tile> GetTilesAffected()
        {
            return tilesInRange;
        }

        public override List<Entity> GetEntitiesAffected()
        {
            return entitiesInRange;
        }

        public void UpdateCost()
        {
            
        }

        public void UpdateDamage()
        {
            
        }

        public void UpdateRange()
        {

        }

        public void UpdateTilesAffected()
        {
            tilesInRange.Clear();
            entitiesInRange.Clear();

            var nextTile = Game.currentGame.board.NextTileInDirection(associatedEntity.currentTile, associatedEntity.direction);
            var entityRanged = Game.currentGame.board.GetFirstEntityInDirectionWithRange(nextTile, associatedEntity.direction, range, out Tile[] tilesRanged);
            tilesInRange.AddRange(tilesRanged);

            if (entityRanged != Entity.noEntity)
            {
                entitiesInRange.Add(entityRanged);
            }
        }
        
        public override void UpdateEntitiesAffected()
        {
            
        }
    }
}