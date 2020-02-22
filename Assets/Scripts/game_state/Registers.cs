using System;

public struct Register
{
	public static int MAX_VALUE => 7;

	int _value;
	public int Value
	{
		get => _value;
		set => _value = mod(value);
	}

	static int mod (int x) => ((x % MAX_VALUE) + MAX_VALUE) % MAX_VALUE;

	public Register (int value)
	{
		_value = mod(value);
	}

	public override string ToString ()
	{
		return Value.ToString();
	}

	public static implicit operator int (Register r) => r.Value;
	public static explicit operator Register (int i) => new Register(i);

	public static Register operator + (Register r) => r;
	public static Register operator - (Register r) => new Register(-r.Value);

	public static Register operator + (Register r, double d) => new Register(r.Value + (int) d);
	public static Register operator - (Register r, double d) => r + (-d);

	public static Register operator * (Register r, double d) => new Register((int) (r.Value * d));
	public static Register operator / (Register r, double d)
	{
		if (d == 0) throw new DivideByZeroException();

		return new Register((int) (r.Value / d));
	}
}

public struct RegVec
{
	public Register R0, R1, R2, R3;

	public static RegVec Zero => new RegVec();
	public static RegVec Max => new RegVec(Register.MAX_VALUE, Register.MAX_VALUE, Register.MAX_VALUE, Register.MAX_VALUE);
	public static RegVec One => new RegVec(1, 1, 1, 1);

	public static RegVec Basis0 = new RegVec(1, 0, 0, 0);
	public static RegVec Basis1 = new RegVec(0, 1, 0, 0);
	public static RegVec Basis2 = new RegVec(0, 0, 1, 0);
	public static RegVec Basis3 = new RegVec(0, 0, 0, 1);

	public Register this [int i]
	{
		get
		{
			switch (i)
			{
				case 0: return R0;
				case 1: return R1;
				case 2: return R2;
				case 3: return R3;

				default:
					throw new IndexOutOfRangeException("Invalid RegVec index!");
			}
		}

		set
		{
			switch (i)
			{
				case 0: R0.Value = value; break;
				case 1: R1.Value = value; break;
				case 2: R2.Value = value; break;
				case 3: R3.Value = value; break;

				default:
					throw new IndexOutOfRangeException("Invalid RegVec index!");
			}
		}
	}

	public RegVec (int r0, int r1, int r2, int r3)
	{
		R0 = (Register) r0;
		R1 = (Register) r1;
		R2 = (Register) r2;
		R3 = (Register) r3;
	}

	public override string ToString ()
	{
		return $"rv({R0},{R1},{R2},{R3})";
	}

	public static RegVec operator + (RegVec v) => v;
	public static RegVec operator - (RegVec v) => new RegVec(-v.R0, -v.R1, -v.R2, -v.R3);

	public static RegVec operator + (RegVec a, RegVec b) => new RegVec(a.R0 + b.R0, a.R1 + b.R1, a.R2 + b.R2, a.R3 + b.R3);
	public static RegVec operator - (RegVec a, RegVec b) => a + (-b);

	public static RegVec operator * (RegVec v, double d) => new RegVec(v.R0 * d, v.R1 * d, v.R2 * d, v.R3 * d);
	public static RegVec operator / (RegVec v, double d)
	{
		if (d == 0) throw new DivideByZeroException();

		return new RegVec(v.R0 / d, v.R1 / d, v.R2 / d, v.R3 / d);
	}
}
