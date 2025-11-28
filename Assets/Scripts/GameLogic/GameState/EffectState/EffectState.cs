using System.Collections.Generic;

namespace GameLogic{

    namespace GameState{

        //TODO: targets
        public class EffectState{

            public Dictionary<string, object> metaData
            {
                get;
                set;
            }

            public string id
            {
                get;
                set;
            }
            
            public string effectName
            {
                get;
                set;
            }
            
            public string effectText
            {
                get;
                set;
            }
            
            public bool displayOnUI
            {
                get;
                set;
            }

            public bool dealsDamage
            {
                get;
                set;
            }
            
            public DamageState damage
            {
                get;
                set;
            }
            
            public bool hasCost
            {
                get;
                set;
            }
            
            public CostState cost
            {
                get;
                set;
            }

            public bool costCanBePaid
            {
                get;
                set;
            }
            
            public bool hasRange
            {
                get;
                set;
            }
            
            public int range
            {
                get; 
                set;
            }
            
            public bool affectsEntities{
                get; 
                set;
            }

            //TODO : dictionnary by player num
            public Dictionary<uint, List<uint>> entitiesAffected
            {
                get;
                set;
            }

            public bool affectsTiles{
                get; 
                set;
            }

            public List<uint> tilesAffected
            {
                get;
                set;
            }

            public bool isActivableEffect{
                get; 
                set;
            }
            
            public bool canBeActivated
            {
                get;
                set;
            }
            
            public bool isActivableWithEntityTargetEffect
            {
                get;
                set;
            }
            
            public Dictionary<uint, List<uint>> possibleEntityTargets
            {
                get;
                set;
            }
            
            public bool isActivableWithTileTargetEffect
            {
                get;
                set;
            }
            
            public List<uint> possibleTileTargets
            {
                get;
                set;
            }

            public bool givesTempBuff
            {
                get;
                set;
            }

            public List<BuffState> buffs
            {
                get;
                set;
            }

            
        }
    }
}