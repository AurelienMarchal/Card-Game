public struct Cost{
    
    public Heart[] heartCost{
        get;
        private set;
    }

    public int movementCost{
        get;
        private set;
    }

    public Cost(Heart[] hearts, int movement){
        heartCost = hearts;
        movementCost = movement;
    }

    public static Cost noCost = new Cost(new Heart[0], 0);
}
