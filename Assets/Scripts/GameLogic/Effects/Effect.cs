using System;
using System.Collections.Generic;
using GameLogic.GameAction;
using UnityEngine;


namespace GameLogic{

    using GameAction;
    using GameBuff;
    using GameState;

    namespace GameEffect{
        //TODO: Effects with targets
        public class Effect{
            
            public Guid id{
                get;
                private set;
            }

            public bool displayOnUI
            {
                get;
                private set;
            }

            [Obsolete]
            public List<EntityBuff> entityBuffs
            {
                get;
                private set;
            }

            public Effect(bool displayOnUI = true){
                this.displayOnUI = displayOnUI;
                id = Guid.NewGuid();
                entityBuffs = new List<EntityBuff>();
            }

            protected EffectActivatedAction effectActivatedAction;

            public virtual bool CanBeActivated(){
                return false;
            }

            protected virtual void Activate(){
            }

            public virtual bool TryToActivate(){
                var result = CanBeActivated();
                if(result){
                    Activate();
                }
                return result;
            }

            public virtual bool TryToCreateEffectActivatedAction(out EffectActivatedAction effectActivatedAction, Action requiredAction = null){
                effectActivatedAction = new EffectActivatedAction(this, requiredAction);
                var canBeActivated = CanBeActivated();
                if(canBeActivated){
                    this.effectActivatedAction = effectActivatedAction;
                    Game.currentGame.PileAction(effectActivatedAction);
                }
                
                return canBeActivated;
            }

            public virtual bool Trigger(Action action){
                return false;
            }

            public virtual string GetEffectText(){
                return "No Effect";
            }

            [Obsolete]
            public virtual void GetTilesAndEntitiesAffected(out Entity[] entitiesAffected, out Tile[] tilesAffected)
            {

                tilesAffected = new Tile[0];
                entitiesAffected = new Entity[0];

            }

            [Obsolete]
            public virtual EffectState ToEffectState()
            {
                EffectState effectState = new EffectState();
                effectState.canBeActivated = CanBeActivated();
                effectState.effectText = GetEffectText();

                effectState.entitiesAffectedNums = new List<uint>();
                effectState.tilesAffectedNums = new List<uint>();

                GetTilesAndEntitiesAffected(out Entity[] entitiesAffected, out Tile[] tilesAffected);

                foreach (Entity entity in entitiesAffected)
                {
                    effectState.entitiesAffectedNums.Add(entity.num);
                }

                foreach (Tile tile in tilesAffected)
                {
                    effectState.tilesAffectedNums.Add(tile.num);
                }

                return effectState;
            }
        }
    }
}