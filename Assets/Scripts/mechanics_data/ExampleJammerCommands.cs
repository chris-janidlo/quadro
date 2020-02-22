using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleJammerCommandA : Command
{
	protected override CommandData data => new CommandData
    {
        Name = "A",
        Description = "",
        Color = Color.clear,
        ComboData = new CommandInputBools
        {
            A = true,
            B = true,
            C = true,
            D = true,
            E = true,
            F = true,
            G = true
        }
    };

	public override void DoEffect (CPU currentCPU)
	{
		Debug.Log("not implemented");
	}
}

public class ExampleJammerCommandB : Command
{
	protected override CommandData data => new CommandData
    {
        Name = "B",
        Description = "",
        Color = Color.clear,
        ComboData = new CommandInputBools
        {
            A = true,
            B = true,
            C = true,
            D = true,
            E = true,
            F = true,
            G = true
        }
    };

	public override void DoEffect (CPU currentCPU)
	{
		Debug.Log("not implemented");
	}
}

public class ExampleJammerCommandC : Command
{
	protected override CommandData data => new CommandData
    {
        Name = "C",
        Description = "",
        Color = Color.clear,
        ComboData = new CommandInputBools
        {
            A = true,
            B = true,
            C = true,
            D = true,
            E = true,
            F = true,
            G = true
        }
    };

	public override void DoEffect (CPU currentCPU)
	{
		Debug.Log("not implemented");
	}
}

public class ExampleJammerCommandD : Command
{
	protected override CommandData data => new CommandData
    {
        Name = "D",
        Description = "",
        Color = Color.clear,
        ComboData = new CommandInputBools
        {
            A = true,
            B = true,
            C = true,
            D = true,
            E = true,
            F = true,
            G = true
        }
    };

	public override void DoEffect (CPU currentCPU)
	{
		Debug.Log("not implemented");
	}
}

public class ExampleJammerCommandE : Command
{
	protected override CommandData data => new CommandData
    {
        Name = "E",
        Description = "",
        Color = Color.clear,
        ComboData = new CommandInputBools
        {
            A = true,
            B = true,
            C = true,
            D = true,
            E = true,
            F = true,
            G = true
        }
    };

	public override void DoEffect (CPU currentCPU)
	{
		Debug.Log("not implemented");
	}
}

public class ExampleJammerCommandF : Command
{
	protected override CommandData data => new CommandData
    {
        Name = "F",
        Description = "",
        Color = Color.clear,
        ComboData = new CommandInputBools
        {
            A = true,
            B = true,
            C = true,
            D = true,
            E = true,
            F = true,
            G = true
        }
    };

	public override void DoEffect (CPU currentCPU)
	{
		Debug.Log("not implemented");
	}
}

public class ExampleJammerCommandG : Command
{
	protected override CommandData data => new CommandData
    {
        Name = "G",
        Description = "",
        Color = Color.clear,
        ComboData = new CommandInputBools
        {
            A = true,
            B = true,
            C = true,
            D = true,
            E = true,
            F = true,
            G = true
        }
    };

	public override void DoEffect (CPU currentCPU)
	{
		Debug.Log("not implemented");
	}
}
