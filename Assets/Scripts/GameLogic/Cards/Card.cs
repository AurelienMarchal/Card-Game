using System.Collections;
using System.Collections.Generic;

namespace GameLogic{
    using System;
    using GameAction;
    using GameEffect;
    using GameState;

    public class Card {
        
        public uint num
        {
            get;
            private set;
        }

        public bool needsEntityTarget {
            get;
            protected set;
        }

        public bool needsTileTarget{
            get;
            protected set;
        }

        public Cost cost{
            get;
            protected set;
        }

        protected EntityPlayCardAction entityPlayCardAction;

        public ActivableEffect activableEffect{
            get;
            private set;
        }

        public Card(uint num, ActivableEffect activableEffect)
        {
            this.num = num;
            this.activableEffect = activableEffect;
            cost = activableEffect.cost;
        }

        [Obsolete]
        public Card(ScriptableActivableEffect scriptableActivableEffect)
        {
            activableEffect = scriptableActivableEffect.GetActivableEffect();
            cost = scriptableActivableEffect.GetActivableEffect().cost;
        }

        public virtual bool CanBeActivated(Entity caster, Tile targetTile = null, Entity targetEntity = null){
            activableEffect.associatedEntity = caster;
            var canBeActivated = activableEffect.CanBeActivated();
            activableEffect.associatedEntity = Entity.noEntity;
            return canBeActivated;
        }

        protected virtual bool Activate(Entity caster, Tile targetTile = null, Entity targetEntity = null)
        {
            activableEffect.associatedEntity = caster;
            return activableEffect.TryToCreateEffectActivatedAction(entityPlayCardAction, out EffectActivatedAction _);
        }

        public virtual string GetText()
        {
            return activableEffect.GetEffectText();
        }

        public virtual string GetCardName()
        {
            return activableEffect.GetType().ToString();
        }

        public bool TryToActivate(Entity caster, Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity){
            var canBeActivated = CanBeActivated(caster, targetTile, targetEntity);
            
            if(canBeActivated){
                
                return Activate(caster, targetTile, targetEntity);
            }
            return canBeActivated;
        }

        public virtual List<Entity> PossibleEntityCasters(){
            var entityTargetList = new List<Entity>();

            return entityTargetList;
        }

        public virtual List<Tile> PossibleTileTargets(Entity caster){
            var tileTargetList = new List<Tile>();

            return tileTargetList;
        }

        public virtual List<Entity> PossibleEntityTargets(Entity caster){
            var entityTargetList = new List<Entity>();

            return entityTargetList;
        }

        public CardState ToCardState(){
            CardState cardState= new CardState();
            cardState.cardName = GetCardName();
            cardState.costState = cost.ToCostState();
            cardState.text = GetText();
            cardState.needsEntityTarget = needsEntityTarget;
            cardState.needsTileTarget = needsTileTarget;
            cardState.activableEffectState = activableEffect.ToEffectState();
            cardState.num = num;
            return cardState;
        }

    }
}
