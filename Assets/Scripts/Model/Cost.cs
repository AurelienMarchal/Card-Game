using System;

[Serializable]
public struct Cost{
    
    public Heart[] heartCost;

    public int movementCost;

    public Cost(Heart[] hearts, int movement){
        heartCost = hearts;
        movementCost = movement;
    }

    public static Cost noCost = new Cost(new Heart[0], 0);
}
