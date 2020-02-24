using System;

public struct RegVec
{
	public const int MAX_VALUE = 7;

	static int mod (int x) => ((x % (MAX_VALUE + 1)) + (MAX_VALUE + 1)) % (MAX_VALUE + 1);

	public static RegVec Zero => new RegVec();
	public static RegVec Max => new RegVec(MAX_VALUE, MAX_VALUE, MAX_VALUE, MAX_VALUE);
	public static RegVec One => new RegVec(1, 1, 1, 1);

	public static RegVec Basis0 = new RegVec(1, 0, 0, 0);
	public static RegVec Basis1 = new RegVec(0, 1, 0, 0);
	public static RegVec Basis2 = new RegVec(0, 0, 1, 0);
	public static RegVec Basis3 = new RegVec(0, 0, 0, 1);

	int _r0, _r1, _r2, _r3;

	public int R0
	{
		get => _r0;
		set => _r0 = mod(value);
	}

	public int R1
	{
		get => _r1;
		set => _r1 = mod(value);
	}

	public int R2
	{
		get => _r2;
		set => _r2 = mod(value);
	}

	public int R3
	{
		get => _r3;
		set => _r3 = mod(value);
	}

	public int this [int i]
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
				case 0: R0 = mod(value); break;
				case 1: R1 = mod(value); break;
				case 2: R2 = mod(value); break;
				case 3: R3 = mod(value); break;

				default:
					throw new IndexOutOfRangeException("Invalid RegVec index!");
			}
		}
	}

	public RegVec (int r0, int r1, int r2, int r3)
	{
		_r0 = mod(r0);
		_r1 = mod(r1);
		_r2 = mod(r2);
		_r3 = mod(r3);
	}

	public override string ToString ()
	{
		return $"r({R0},{R1},{R2},{R3})";
	}

	public static RegVec SafeAdd (RegVec a, RegVec b)
	{
		return new RegVec
		(
			Math.Min(a.R0 + b.R0, MAX_VALUE),
			Math.Min(a.R1 + b.R1, MAX_VALUE),
			Math.Min(a.R2 + b.R2, MAX_VALUE),
			Math.Min(a.R3 + b.R3, MAX_VALUE)
		);
	}

	public static RegVec operator + (RegVec v) => v;
	public static RegVec operator - (RegVec v) => new RegVec(-v.R0, -v.R1, -v.R2, -v.R3);

	public static RegVec operator + (RegVec a, RegVec b) => new RegVec(a.R0 + b.R0, a.R1 + b.R1, a.R2 + b.R2, a.R3 + b.R3);
	public static RegVec operator - (RegVec a, RegVec b) => a + (-b);

	public static RegVec operator * (RegVec v, double d) => new RegVec((int) (v.R0 * d), (int) (v.R1 * d), (int) (v.R2 * d), (int) (v.R3 * d));
	public static RegVec operator * (double d, RegVec v) => v * d;

	public static RegVec operator / (RegVec v, double d)
	{
		if (d == 0) throw new DivideByZeroException();

		return new RegVec((int) (v.R0 / d), (int) (v.R1 / d), (int) (v.R2 / d), (int) (v.R3 / d));
	}
}
