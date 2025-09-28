using System;
using System.Collections.Generic;
using GameLogic.GameAction;
using UnityEngine;


namespace GameLogic{

    using GameAction;
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

            public Effect(bool displayOnUI = true)
            {
                this.displayOnUI = displayOnUI;
                id = Guid.NewGuid();
            }

            public virtual string GetEffectName()
            {
                return "No Name";
            }
            
            public virtual string GetEffectText()
            {
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
                //effectState.canBeActivated = CanBeActivated();
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