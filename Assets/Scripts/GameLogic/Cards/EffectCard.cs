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

        public EffectCard(uint num, Player player, Cost cost, Effect effect) : base(num, player, cost) 
        {
            this.effect = effect;
        }

        public override bool CanBeActivated(Tile targetTile = null, Entity targetEntity = null)
        {
            //var canBeActivated = effect.CanBeActivated();
            //return canBeActivated;
            return false;
        }

        protected override bool Activate(Tile targetTile = null, Entity targetEntity = null)
        {

            //return effect.TryToCreateEffectActivatesAction(out EffectActivatesAction _);
            return false;
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