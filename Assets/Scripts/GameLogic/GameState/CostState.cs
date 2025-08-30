using System.Collections.Generic;

namespace GameLogic{

    namespace GameState
    {
        public class CostState
        {
            public List<HeartType> heartCost
            {
                get;
                set;
            }

            public int mouvementCost
            {
                get;
                set;
            }
            
            public int manaCost
            {
                get;
                set;
            }

            public Dictionary<HeartType, int> GetHeartTypeDict()
            {
                var dict = new Dictionary<HeartType, int>();

                foreach (var heart in heartCost)
                {
                    if (!dict.ContainsKey(heart))
                    {
                        dict.Add(heart, 1);
                    }
                    else
                    {
                        dict[heart] += 1;
                    }
                }

                return dict;
            }
        }
    }
}