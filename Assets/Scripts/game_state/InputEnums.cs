using System;
using crass;

public struct InputFrame
{
    public readonly ComInput? ComInput;
    public readonly CPUSwitchInput? CPUSwitchInput;
    public readonly bool CPUExecuteInput;

    public bool FrameIsEmpty => ComInput == null && CPUSwitchInput == null && !CPUExecuteInput;

    public InputFrame (ComInput? com, CPUSwitchInput? cpuSwitch, bool cpuExecute)
    {
        ComInput = com;
        CPUSwitchInput = cpuSwitch;
        CPUExecuteInput = cpuExecute;
    }
}



public enum ComInput
{
    A, B, C, D, E, F, G
}

public class ComInputBox<T>
{
    public T A, B, C, D, E, F, G;

    public T this [ComInput input]
    {
        get
        {
            switch (input)
            {
                case ComInput.A: return A;
                case ComInput.B: return B;
                case ComInput.C: return C;
                case ComInput.D: return D;
                case ComInput.E: return E;
                case ComInput.F: return F;
                case ComInput.G: return G;
                default: throw new ArgumentException("unexpected ComInput value " + input);
            }
        }
        set
        {
            switch (input)
            {
                case ComInput.A: A = value; break;
                case ComInput.B: B = value; break;
                case ComInput.C: C = value; break;
                case ComInput.D: D = value; break;
                case ComInput.E: E = value; break;
                case ComInput.F: F = value; break;
                case ComInput.G: G = value; break;
                default: throw new ArgumentException("unexpected ComInput value " + input);
            }
        }
    }
}

[Serializable]
public class ComInputStrings : ComInputBox<string> {}

[Serializable]
public class ComInputBools : ComInputBox<bool> {}



public enum CPUSwitchInput
{
    Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine
}

public class CPUSwitchInputBox<T>
{
    public T Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine;

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
                case CPUSwitchInput.Four: return Four;
                case CPUSwitchInput.Five: return Five;
                case CPUSwitchInput.Six: return Six;
                case CPUSwitchInput.Seven: return Seven;
                case CPUSwitchInput.Eight: return Eight;
                case CPUSwitchInput.Nine: return Nine;
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
                case CPUSwitchInput.Four: Four = value; break;
                case CPUSwitchInput.Five: Five = value; break;
                case CPUSwitchInput.Six: Six = value; break;
                case CPUSwitchInput.Seven: Seven = value; break;
                case CPUSwitchInput.Eight: Eight = value; break;
                case CPUSwitchInput.Nine: Nine = value; break;
                default: throw new ArgumentException("unexpected CPUSwitchInput value " + input);
            }
        }
    }
}

[Serializable]
public class CPUSwitchInputStrings : CPUSwitchInputBox<string> {}
