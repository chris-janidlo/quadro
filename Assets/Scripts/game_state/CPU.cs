using System;

public class CPU
{
	public abstract class Instruction
	{
		public abstract string Name { get; }
		// based on the values of RegVec v, manipulate the game state as desired. return value: what the state of the CPU's registers should be after executing
		public abstract RegVec DoBehavior (RegVec v, Player owner);
	}

	public RegVec Registers = new RegVec();
	public Instruction Instr = null;

	Player owner;

	public CPU (Player owner)
	{
		this.owner = owner;
	}

	public void Execute ()
	{
		if (Instr == null) throw new InvalidOperationException("Instr is null. caller needs to check that it isn't null before calling Execute");

		Registers = Instr.DoBehavior(Registers, owner);
		Instr = null;
	}

	public void FlushRegisters ()
	{
		Registers = RegVec.Zero;
	}
}
