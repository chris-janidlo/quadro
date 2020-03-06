using System;

public enum CPUSwitchInput
{
    Zero, One, Two, Three
}

public class CPUSwitchInputBox<T>
{
    public T Zero, One, Two, Three;

    public T this [CPUSwitchInput input]
    {
        get
        {
            switch (input)
            {
                case CPUSwitchInput.Zero: return Zero;
                case CPUSwitchInput.One: return One;
                case CPUSwitchInput.Two: return Two;
                case CPUSwitchInput.Three: return Three;
                default: throw new ArgumentException("unexpected CPUSwitchInput value " + input);
            }
        }
        set
        {
            switch (input)
            {
                case CPUSwitchInput.Zero: Zero = value; break;
                case CPUSwitchInput.One: One = value; break;
                case CPUSwitchInput.Two: Two = value; break;
                case CPUSwitchInput.Three: Three = value; break;
                default: throw new ArgumentException("unexpected CPUSwitchInput value " + input);
            }
        }
    }
}

[Serializable]
public class CPUSwitchInputStrings : CPUSwitchInputBox<string> {}
