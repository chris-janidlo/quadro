using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleJammerCommandC : Command
{
	protected override CommandData _data => new CommandData
    {
        Name = "R0 Up",
        Description = "Increases R0 by 3 (WARNING: can cause overflow)",
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

	public override void DoEffect (CPU cpu)
	{
        cpu.Registers.R0 += 3;
	}
}

public class ExampleJammerCommandD : Command
{
	protected override CommandData _data => new CommandData
    {
        Name = "R1 rMin",
        Description = "Sets R1 to minimum(r0, r2, r3)",
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

	public override void DoEffect (CPU cpu)
	{
        cpu.Registers = new RegVec
        (
            cpu.Registers.R0,
            Mathf.Min(cpu.Registers.R0, cpu.Registers.R2, cpu.Registers.R3),
            cpu.Registers.R2,
            cpu.Registers.R3
        );
	}
}

public class ExampleJammerCommandE : Command
{
	protected override CommandData _data => new CommandData
    {
        Name = "R2 Up",
        Description = "Increases R2 by 3 (WARNING: can cause overflow)",
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

	public override void DoEffect (CPU cpu)
	{
        cpu.Registers.R2 += 3;
	}
}

public class ExampleJammerCommandF : Command
{
	protected override CommandData _data => new CommandData
    {
        Name = "R3 HUP",
        Description = "Increases R3 by R0 / 2",
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

	public override void DoEffect (CPU cpu)
	{
        cpu.Registers = RegVec.SafeAdd(cpu.Registers, RegVec.Basis3 * cpu.Registers.R0 / 2);
	}
}

public class ExampleJammerCommandG : Command
{
	protected override CommandData _data => new CommandData
    {
        Name = "Instr HIT",
        Description = "Sets instr: deal [R0 * (R1 + R3) / 2 + R2] damage",
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

	class instr : CPU.Instruction
	{
		public override string Name => "HIT";

		public override RegVec DoBehavior (RegVec v, Player owner)
		{
            owner.Opponent.TakeShieldedDamage(v.R0 * (v.R2 + v.R1) / 2 + v.R3);
            return RegVec.Zero;
		}
	}

	public override void DoEffect (CPU cpu) => cpu.Instr = new instr();
}

public class ExampleJammerCommandA : Command
{
	protected override CommandData _data => new CommandData
    {
        Name = "Instr BLK",
        Description = "Sets instr: add [R0 * (R1 + R3) + R2] armor",
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

	class instr : CPU.Instruction
	{
		public override string Name => "BLK";

		public override RegVec DoBehavior (RegVec v, Player owner)
		{
            owner.Armor.Value = v.R0 * (v.R2 + v.R1) + v.R3;
            return RegVec.Zero;
		}
	}

	public override void DoEffect (CPU cpu) => cpu.Instr = new instr();
}

public class ExampleJammerCommandB : Command
{
	protected override CommandData _data => new CommandData
    {
        Name = "Instr BPM",
        Description = "Sets instr: increase BPM by R1",
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

	class instr : CPU.Instruction
	{
		public override string Name => "BPM";

		public override RegVec DoBehavior (RegVec v, Player owner)
		{
            owner.Track.BSteps.Value += v.R1;
            return v;
		}
	}

	public override void DoEffect (CPU cpu) => cpu.Instr = new instr();
}
