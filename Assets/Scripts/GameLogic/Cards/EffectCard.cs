using System.Collections;
using System.Collections.Generic;

namespace GameLogic
{
    using System;
    using GameAction;
    using GameEffect;
    using GameState;

    public class EffectCard : Card
    {
        public Effect effect
        {
            get;
            private set;
        }

        public EffectCard(uint num, Player player, Cost cost, Effect effect, bool needsEntityTarget = false, bool needsTileTarget = false) : base(num, player, cost, needsEntityTarget, needsTileTarget) 
        {
            this.effect = effect;
        }

        public override bool CanBeActivated(Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity)
        {
            switch (effect)
            {
                case CanBeActivatedWithEntityTargetInterface entityTargetEffect:
                    return entityTargetEffect.CanBeActivatedWithEntityTarget(targetEntity);
                case CanBeActivatedWithTileTargetInterface tileTargetEffect:
                    return tileTargetEffect.CanBeActivatedWithTileTarget(targetTile);
                case CanBeActivatedInterface canBeActivatedEffect:
                    return canBeActivatedEffect.CanBeActivated();
            }

            return false;
        }

        protected override bool Activate(Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity)
        {

            if (targetTile != Tile.noTile)
            {
                var effectActivatesWithTileTargetAction = new EffectActivatesWithTileTargetAction(effect, targetTile);
                Game.currentGame.PileAction(effectActivatesWithTileTargetAction);
                if (effectActivatesWithTileTargetAction.wasPerformed || effectActivatesWithTileTargetAction.wasCancelled)
                {
                    return true;
                }
            }

            if (targetEntity != Entity.noEntity)
            {
                var effectActivatesWithEntityTargetAction = new EffectActivatesWithEntityTargetAction(effect, targetEntity);
                Game.currentGame.PileAction(effectActivatesWithEntityTargetAction);
                if (effectActivatesWithEntityTargetAction.wasPerformed || effectActivatesWithEntityTargetAction.wasCancelled)
                {
                    return true;
                }
            }

            var effectActivatesAction = new EffectActivatesAction(effect);
            Game.currentGame.PileAction(effectActivatesAction);
            if(effectActivatesAction.wasPerformed || effectActivatesAction.wasCancelled)
            {
                return true;
            }

            return false;
        }

        public override List<Entity> PossibleEntityTargets()
        {

            if(effect is CanBeActivatedWithEntityTargetInterface entityTargetEffect)
            {
                return entityTargetEffect.PossibleEntityTargets();
            }
            return base.PossibleEntityTargets();
        }

        public override List<Tile> PossibleTileTargets()
        {   
            if(effect is CanBeActivatedWithTileTargetInterface tileTargetEffect)
            {
                return tileTargetEffect.PossibleTileTargets();
            }
            return base.PossibleTileTargets();
        }

        public override string GetText()
        {
            return effect.GetEffectText();
        }

        public override string GetCardName()
        {
            return effect.GetType().ToString();
        }
    }
}