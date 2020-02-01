using System;
using UnityEngine;

public abstract class BoxedValue<T>
{
    public event Action<T> ValueDidChange;

    public readonly T Min, Max;

    T _value;
    public T Value
    {
        get => _value;
        set
        {
            _value = clamp(value, Min, Max);
            ValueDidChange?.Invoke(_value);
        }
    }

    public BoxedValue (T initial, T min, T max)
    {
        _value = initial;
        Min = min;
        Max = max;
    }

    protected abstract T clamp (T input, T min, T max);
}

public class BoxedInt : BoxedValue<int>
{
	public BoxedInt (int initial, int min, int max) : base(initial, min, max) {}
	protected override int clamp (int input, int min, int max) => Mathf.Clamp(input, min, max);
}

public class BoxedFloat : BoxedValue<float>
{
	public BoxedFloat (float initial, float min, float max) : base(initial, min, max) {}
	protected override float clamp (float input, float min, float max) => Mathf.Clamp(input, min, max);
}

public class BoxedDouble : BoxedValue<double>
{
	public BoxedDouble (double initial, double min, double max) : base(initial, min, max) {}
	protected override double clamp (double input, double min, double max) => input > max ? max : (input < min ? min : input);
}