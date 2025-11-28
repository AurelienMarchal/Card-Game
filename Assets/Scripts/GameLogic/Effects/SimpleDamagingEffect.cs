using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{

    using GameEffect;
    using GameAction;

    public class SimpleDamagingEffect : EntityEffect, AffectsTilesInterface, DealsDamageInterface, HasCostInterface, HasRangeInterface, CanBeActivatedInterface
    {

        private Cost baseCost
        {
            get; set;
        }

        private Damage baseDamage
        {
            get; set;
        }

        private int baseRange
        {
            get; set;
        }

        List<Tile> tilesInRange;

        List<Entity> entitiesInRange;

        public SimpleDamagingEffect(Entity entity, Cost baseCost, Damage baseDamage, int baseRange, bool displayOnUI = true) : base(entity, displayOnUI)
        {
            this.baseCost = baseCost;
            this.baseDamage = baseDamage;
            this.baseRange = baseRange;
            tilesInRange = new List<Tile>();
            entitiesInRange = new List<Entity>();
        }

        void CanBeActivatedInterface.Activate()
        {

            if (entitiesInRange.Count > 0 && entitiesInRange[0] != Entity.noEntity)
            {
                var entityHit = entitiesInRange[0];
                var tileReached = entityHit.currentTile;
                Game.currentGame.PileAction(new EntityTakeDamageAction(entityHit, baseDamage));
            }
        }

        public bool CanBeActivated()
        {
            return true;
        }

        public override string GetEffectText()
        {
            return $"Deal {GetDamage()} with baseRange {GetRange()}";
        }

        public override string GetEffectName()
        {
            return $"Simple damaging effect";
        }

        public bool CheckTriggerToActivate(Action action)
        {
            return false;
        }

        public System.Type[] ActionTypeTriggersToActivate()
        {
            return null;
        }

        public bool CheckTriggerToUpdateTilesAffected(Action action)
        {
            switch (action)
            {
                case StartGameAction startGameAction:
                    return startGameAction.wasPerformed;
                case PlayerSpawnEntityAction playerSpawnEntityAction:
                    return playerSpawnEntityAction.wasPerformed;
                case EntityMoveAction entityMoveAction:
                    return entityMoveAction.wasPerformed;
                case EntityDieAction entityDieAction:
                    return entityDieAction.wasPerformed;
            }

            return false;
        }

        public System.Type[] ActionTypeTriggersToUpdateTilesAffected()
        {
            return new System.Type[4]{typeof(PlayerSpawnEntityAction), typeof(EntityMoveAction), typeof(EntityDieAction), typeof(StartGameAction)};
        }
        


        public bool CheckTriggerToUpdateCost(Action action)
        {
            return false;
        }

        public System.Type[] ActionTypeTriggersToUpdateCost()
        {
            return null;
        }

        public bool CheckTriggerToUpdateDamage(Action action)
        {
            return false;
        }

        public System.Type[] ActionTypeTriggersToUpdateDamage()
        {
            return null;
        }

        public bool CheckTriggerToUpdateRange(Action action)
        {
            return false;
        }

        public System.Type[] ActionTypeTriggersToUpdateRange()
        {
            return null;
        }

        public Cost GetCost()
        {
            return baseCost;
        }

        public Damage GetDamage()
        {
            return baseDamage;
        }

        public int GetRange()
        {
            return baseRange;
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
            var entityRanged = Game.currentGame.board.GetFirstEntityInDirectionWithRange(nextTile, associatedEntity.direction, baseRange, out Tile[] tilesRanged);
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