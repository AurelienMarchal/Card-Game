namespace GameLogic{
    public enum Direction{
        North, South, East, West
    }

    public static class DirectionsExtensions{
        public static float ToAngle(this Direction direction)
        {
            switch(direction){
                case Direction.North: return 0f;
                case Direction.South: return 180f;
                case Direction.East: return 90f;
                case Direction.West: return 270f;
                default: return 0f;
            }
        }

        public static Direction FromCoordinateDifference(int difX, int difY)
        {
            if(difX == 0){
                if(difY > 0){
                    return Direction.North;
                }
                else{
                    return Direction.South;
                }
            }
            else if(difX > 0){
                return Direction.East;
            }
            else{
                return Direction.West;
            }
        }
    }
}
