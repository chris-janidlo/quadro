using System.ComponentModel;

public enum ComInput
{
    Up = InputDirection.Up,
    Left = InputDirection.Left,
    Down = InputDirection.Down,
    Right = InputDirection.Right,
    Cast
}

public enum InputDirection
{
    Up, Left, Down, Right
}

public static class InputDirectionExtensions
{
    public static char ToArrow (this InputDirection direction)
    {
        switch (direction)
        {
            // these seem to be double width
            case InputDirection.Up:
                return '↑';
            case InputDirection.Left:
                return '←';
            case InputDirection.Down:
                return '↓';
            case InputDirection.Right:
                return '→';
            default:
                throw new InvalidEnumArgumentException($"unexpected Direction {direction}");
        }
    }
}

public class InputDirectionBox<T>
{
    public T Up, Left, Down, Right;

    public T this[InputDirection direction]
    {
        get
        {
            switch (direction)
            {
                case InputDirection.Up:
                    return Up;
                case InputDirection.Left:
                    return Left;
                case InputDirection.Down:
                    return Down;
                case InputDirection.Right:
                    return Right;
                default:
                    throw new InvalidEnumArgumentException($"unexpected Direction {direction}");
            }
        }
    }
}
