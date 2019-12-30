using System.ComponentModel;

public enum Direction
{
    Up, Left, Down, Right
}

public class DirectionBox<T>
{
    public T Up, Left, Down, Right;

    public T this[Direction direction]
    {
        get
        {
            switch (direction)
            {
                case Direction.Up:
                    return Up;
                case Direction.Left:
                    return Left;
                case Direction.Down:
                    return Down;
                case Direction.Right:
                    return Right;
                default:
                    throw new InvalidEnumArgumentException($"unexpected Direction {direction}");
            }
        }
    }
}
