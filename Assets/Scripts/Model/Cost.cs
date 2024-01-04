using System;
using System.Collections.Generic;

[Serializable]
public struct Cost{
    
    public HeartType[] heartCost;

    public int manaCost;

    public Cost(HeartType[] hearts, int mana){
        heartCost = hearts;
        manaCost = mana;
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
