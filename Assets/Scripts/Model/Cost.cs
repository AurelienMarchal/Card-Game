using System;
using System.Collections.Generic;

[Serializable]
public struct Cost{
    
    public HeartType[] heartCost;

    public int mouvementCost;

    public Cost(HeartType[] hearts, int mouvement){
        heartCost = hearts;
        mouvementCost = mouvement;
    }

    public static Cost noCost = new Cost(new HeartType[0], 0);


    public Dictionary<HeartType, int> GetHeartTypeDict(){
        var dict = new Dictionary<HeartType, int>();

        foreach(var heart in heartCost){
            if(!dict.ContainsKey(heart)){
                dict.Add(heart, 1);
            }
            else{
                dict[heart] += 1;
            }
        }

        return dict;
    }
}
