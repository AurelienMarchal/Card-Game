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


        public Card(uint num, Cost cost, bool needsEntityTarget = false, bool needsTileTarget = false)
        {
            this.num = num;
            this.cost = cost;
            this.needsEntityTarget = needsEntityTarget;
            this.needsTileTarget = needsTileTarget;
        }

        public virtual bool CanBeActivated(Tile targetTile = null, Entity targetEntity = null){
            return false;
        }

        protected virtual bool Activate(Tile targetTile = null, Entity targetEntity = null)
        {
            return false;
        }

        public virtual string GetText()
        {
            return "No text  Card";
        }

        public virtual string GetCardName()
        {
            return "No name card";
        }

        public bool TryToActivate(Tile targetTile = Tile.noTile, Entity targetEntity = Entity.noEntity){
            var canBeActivated = CanBeActivated(targetTile, targetEntity);
            
            if(canBeActivated){
                
                return Activate(targetTile, targetEntity);
            }
            return canBeActivated;
        }

        public virtual List<Tile> PossibleTileTargets(){
            var tileTargetList = new List<Tile>();

            return tileTargetList;
        }

        public virtual List<Entity> PossibleEntityTargets(){
            var entityTargetList = new List<Entity>();

            return entityTargetList;
        }

        public virtual CardState ToCardState(){
            CardState cardState= new CardState();
            cardState.cardName = GetCardName();
            cardState.costState = cost.ToCostState();
            cardState.text = GetText();

            cardState.possibleEntityTargets = new Dictionary<uint, List<uint>>();
            var possibleEntityTargets = PossibleEntityTargets();
            foreach (var entity in possibleEntityTargets)
            {
                if (!cardState.possibleEntityTargets.ContainsKey(entity.player.playerNum))
                {
                    cardState.possibleEntityTargets.Add(entity.player.playerNum, new List<uint>());
                }
                cardState.possibleEntityTargets[entity.player.playerNum].Add(entity.num);

            }

            var possibleTileTargets = PossibleTileTargets();
            cardState.possibleTileTargets = new List<uint>();

            foreach (var tile in possibleTileTargets)
            {
                cardState.possibleTileTargets.Add(tile.num);
            }

            cardState.needsEntityTarget = needsEntityTarget;
            cardState.needsTileTarget = needsTileTarget;
            cardState.num = num;
            return cardState;
        }

    }
}
