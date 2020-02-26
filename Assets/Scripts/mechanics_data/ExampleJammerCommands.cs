using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleJammerCommandC : Command
{
	protected override CommandData _data => new CommandData
    {
        Name = "R0 Up",
        Description = "Increases R0 by 1",
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

	public override RegVec DoEffect (Player owner, RegVec inputVector)
	{
        return RegVec.SafeAdd(inputVector, RegVec.Basis0);
	}
}

public class ExampleJammerCommandD : Command
{
	protected override CommandData _data => new CommandData
    {
        Name = "R1,2 Up",
        Description = "Increases R1 and R2 by 1",
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

	public override RegVec DoEffect (Player owner, RegVec inputVector)
	{
        return RegVec.SafeAdd(inputVector, RegVec.Basis1 + RegVec.Basis2);
	}
}

public class ExampleJammerCommandE : Command
{
	protected override CommandData _data => new CommandData
    {
        Name = "R3 Up",
        Description = "Increases R3 by 1",
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

	public override RegVec DoEffect (Player owner, RegVec inputVector)
	{
        return RegVec.SafeAdd(inputVector, RegVec.Basis3);
	}
}

public class ExampleJammerCommandF : Command
{
	protected override CommandData _data => new CommandData
    {
        Name = "HIT L",
        Description = "Deal R0 * R1 damage",
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

	public override RegVec DoEffect (Player owner, RegVec inputVector)
	{
        owner.Opponent.Health.Value -= inputVector.R0 * inputVector.R1;
        return inputVector / 2;
	}
}

public class ExampleJammerCommandG : Command
{
	protected override CommandData _data => new CommandData
    {
        Name = "HIT H",
        Description = "Deal R2 * R3 damage",
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

    public override RegVec DoEffect (Player owner, RegVec inputVector)
    {
        owner.Opponent.Health.Value -= inputVector.R2 * inputVector.R3;
        return inputVector / 2;
    }
}

public class ExampleJammerCommandA : Command
{
	protected override CommandData _data => new CommandData
    {
        Name = "BLK",
        Description = "Add R1 * R2 armor",
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

    public override RegVec DoEffect (Player owner, RegVec inputVector)
    {
        owner.Armor.Value += inputVector.R1 * inputVector.R2;
        return RegVec.SafeSubtract(inputVector, RegVec.One);
    }
}

public class ExampleJammerCommandB : Command
{
	protected override CommandData _data => new CommandData
    {
        Name = "RGMIN",
        Description = "Deal 2 register damage to the enemy CPU corresponding to your highest active register (WARNING: can cause underflow)",
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

    public override RegVec DoEffect (Player owner, RegVec inputVector)
    {
        int maxReg = 0;

        for (int i = 1; i < 4; i++)
        {
            if (inputVector[i] > inputVector[maxReg]) maxReg = i;
        }

        owner.Opponent.CPUs[maxReg].Registers -= RegVec.One * 2;
        return inputVector / 3;
    }
}
