using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic{

    namespace GameEffect{

        public class PlayerEffect : Effect{
            
            public Player associatedPlayer{
                get;
                protected set;
            }

            public PlayerEffect(Player player){
                associatedPlayer = player;
            }
        }
    }
}
