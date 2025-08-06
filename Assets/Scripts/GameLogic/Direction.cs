namespace GameLogic{
    public enum Direction{
        North, South, East, West
    }
    
    public enum RelativeDirection{
        Front, Back, Right, Left
    }

    public static class DirectionsExtensions
    {
        public static float ToAngle(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North: return 0f;
                case Direction.South: return 180f;
                case Direction.East: return 90f;
                case Direction.West: return 270f;
                default: return 0f;
            }
        }

        public static Direction FromCoordinateDifference(int difX, int difY)
        {
            if (difX == 0)
            {
                if (difY > 0)
                {
                    return Direction.North;
                }
                else
                {
                    return Direction.South;
                }
            }
            else if (difX > 0)
            {
                return Direction.East;
            }
            else
            {
                return Direction.West;
            }
        }

        public static RelativeDirection RelativeDirectionBetweenDirections(Direction originDirection, Direction goalDirection)
        {
            switch (originDirection)
            {
                case Direction.North:
                    switch (goalDirection)
                    {
                        case Direction.North: return RelativeDirection.Front;
                        case Direction.South: return RelativeDirection.Back;
                        case Direction.East: return RelativeDirection.Right;
                        case Direction.West: return RelativeDirection.Left;
                        default: return RelativeDirection.Front;
                    }
                case Direction.South:
                    switch (goalDirection)
                    {
                        case Direction.North: return RelativeDirection.Back;
                        case Direction.South: return RelativeDirection.Front;
                        case Direction.East: return RelativeDirection.Left;
                        case Direction.West: return RelativeDirection.Right;
                        default: return RelativeDirection.Front;
                    }
                case Direction.East:
                    switch (goalDirection)
                    {
                        case Direction.North: return RelativeDirection.Left;
                        case Direction.South: return RelativeDirection.Right;
                        case Direction.East: return RelativeDirection.Front;
                        case Direction.West: return RelativeDirection.Back;
                        default: return RelativeDirection.Front;
                    }
                case Direction.West:
                    switch (goalDirection)
                    {
                        case Direction.North: return RelativeDirection.Right;
                        case Direction.South: return RelativeDirection.Left;
                        case Direction.East: return RelativeDirection.Back;
                        case Direction.West: return RelativeDirection.Front;
                        default: return RelativeDirection.Front;
                    }
                default: return RelativeDirection.Front;
            }
        }

        public static Direction ChangingDirectionByRelativeDirection(Direction originDirection, RelativeDirection relativeDirection)
        {
            switch (originDirection)
            {
                case Direction.North:
                    switch (relativeDirection)
                    {
                        case RelativeDirection.Front: return Direction.North;
                        case RelativeDirection.Back: return Direction.South;
                        case RelativeDirection.Right: return Direction.East;
                        case RelativeDirection.Left: return Direction.West;
                        default: return Direction.North;
                    }
                case Direction.South:
                    switch (relativeDirection)
                    {
                        case RelativeDirection.Front: return Direction.South;
                        case RelativeDirection.Back: return Direction.North;
                        case RelativeDirection.Right: return Direction.West;
                        case RelativeDirection.Left: return Direction.East;
                        default: return Direction.North;
                    }
                case Direction.East:
                    switch (relativeDirection)
                    {
                        case RelativeDirection.Front: return Direction.East;
                        case RelativeDirection.Back: return Direction.West;
                        case RelativeDirection.Right: return Direction.South;
                        case RelativeDirection.Left: return Direction.North;
                        default: return Direction.North;
                    }
                case Direction.West:
                    switch (relativeDirection)
                    {
                        case RelativeDirection.Front: return Direction.West;
                        case RelativeDirection.Back: return Direction.East;
                        case RelativeDirection.Right: return Direction.North;
                        case RelativeDirection.Left: return Direction.South;
                        default: return Direction.North;
                    }
                default: return Direction.North;
            }
        }
    }
}
