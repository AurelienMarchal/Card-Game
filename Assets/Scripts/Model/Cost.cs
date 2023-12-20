using System;

[Serializable]
public struct Cost{
    
    public HeartType[] heartCost;

    public int movementCost;

    public Cost(HeartType[] hearts, int movement){
        heartCost = hearts;
        movementCost = movement;
    }

    public static Cost noCost = new Cost(new HeartType[0], 0);
}
