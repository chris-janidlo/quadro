using System;

public class CPU
{
	public RegVec Registers = new RegVec();

	Player owner;

	public CPU (Player owner)
	{
		this.owner = owner;
	}

	public void FlushRegisters ()
	{
		Registers = RegVec.Zero;
	}
}
