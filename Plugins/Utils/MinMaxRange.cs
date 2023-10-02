using System;

public abstract class MinMaxRange<T>
{
    public T RangeMin, RangeMax;

    public abstract T Value { get; }

    /// <summary>
    /// Returns a random value between [rangeStart, rangeEnd] using the provided random function.
    /// </summary>
    /// <param name="MinMaxInclusiveRandomFunction">Inclusive random function that takes a minimum and a maximum value as parameters.</param>
    public T GetRandomValue(System.Func<T, T, T> MinMaxInclusiveRandomFunction)
    {
        return MinMaxInclusiveRandomFunction(RangeMin, RangeMax);
    }    
    
    public abstract T RangeMiddle { get; }

    public abstract bool IsInRange(T value);
}

[Serializable]
public class MinMaxRange : MinMaxRange<float>
{
    public override float Value => GetRandomValue((min, max) => Lerp(min, max, (float)new Random().NextDouble()));

    public override float RangeMiddle => RangeMin + ((RangeMax - RangeMin) * 0.5f);

    public override bool IsInRange(float value)
    {
        return !((value > RangeMax) || (value < RangeMin));
    }

    private float Lerp(float a, float b, float t)
    {
        t = Clamp(t, 0f, 1f);
        return (a * (1f - t) + (b * t));
    }

    private T Clamp<T>(T x, T min, T max) where T : System.IComparable
    {
        var _Result = x;
        if (x.CompareTo(max) > 0)
        {
            _Result = max;
        }
        else if (x.CompareTo(min) < 0)
        {
            _Result = min;
        }
        
        return _Result;
    }
}