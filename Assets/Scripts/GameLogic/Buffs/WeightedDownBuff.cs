using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic{

    namespace GameBuff{
        public class WeightedDownBuff : EntityBuff
        {
            public WeightedDownBuff() : base("Weighted Down"){
            }

            public override string GetText()
            {
                return "Cost one more Mouvement to move";
            }

            public override int IsPositive()
            {
                return -1;
            }
        }
    }
}
