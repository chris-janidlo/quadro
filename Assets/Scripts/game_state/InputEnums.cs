using System;
using crass;

public struct InputFrame
{
    public readonly CommandInput? CommandInput;
    public readonly CPUSwitchInput? CPUSwitchInput;
    public readonly bool CPUExecuteInput;

    public bool FrameIsEmpty => CommandInput == null && CPUSwitchInput == null && !CPUExecuteInput;

    public InputFrame (CommandInput? command, CPUSwitchInput? cpuSwitch, bool cpuExecute)
    {
        CommandInput = command;
        CPUSwitchInput = cpuSwitch;
        CPUExecuteInput = cpuExecute;
    }
}



public enum CommandInput
{
    C, D, E, F, G, A, B
}

public class CommandInputBox<T>
{
    public T C, D, E, F, G, A, B;

    public T this [CommandInput input]
    {
        get
        {
            switch (input)
            {
                case CommandInput.C: return C;
                case CommandInput.D: return D;
                case CommandInput.E: return E;
                case CommandInput.F: return F;
                case CommandInput.G: return G;
                case CommandInput.A: return A;
                case CommandInput.B: return B;
                default: throw new ArgumentException("unexpected CommandInput value " + input);
            }
        }
        set
        {
            switch (input)
            {
                case CommandInput.C: C = value; break;
                case CommandInput.D: D = value; break;
                case CommandInput.E: E = value; break;
                case CommandInput.F: F = value; break;
                case CommandInput.G: G = value; break;
                case CommandInput.A: A = value; break;
                case CommandInput.B: B = value; break;
                default: throw new ArgumentException("unexpected CommandInput value " + input);
            }
        }
    }
}

[Serializable]
public class CommandInputStrings : CommandInputBox<string> {}

[Serializable]
public class CommandInputBools : CommandInputBox<bool> {}



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
