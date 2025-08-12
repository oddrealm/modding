using System;

public readonly struct SimTime : IEquatable<SimTime>
{
    public readonly uint Minutes;

    public uint Hours
    {
        get
        {
            return Minutes / 60;
        }
    }

    public uint Days
    {
        get
        {
            return Minutes / 60 / 24;
        }
    }

    public uint Years
    {
        get
        {
            return Minutes / 60 / 24 / 60;
        }
    }

    public SimTime(uint minutes)
    {
        Minutes = minutes;
    }

    public static implicit operator SimTime(int value)
    {
        if (value < 0) value = 0;
        return new SimTime((uint)value);
    }

    public static implicit operator SimTime(uint value)
    {
        return new SimTime(value);
    }

    public static implicit operator uint(SimTime value)
    {
        return value.Minutes;
    }

    public static implicit operator int(SimTime value)
    {
        return (int)value.Minutes;
    }

    public bool Equals(SimTime other)
    {
        return this.Minutes == other.Minutes;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is SimTime))
            return false;

        SimTime other = (SimTime)obj;
        return Minutes == other.Minutes;
    }

    public override int GetHashCode()
    {
        return Minutes.GetHashCode();
    }

    public static bool operator ==(SimTime left, SimTime right)
    {
        return left.Minutes == right.Minutes;
    }

    public static bool operator !=(SimTime left, SimTime right)
    {
        return left.Minutes != right.Minutes;
    }

    public override string ToString()
    {
        return string.Format("[{0}m, {1}h, {2}d, {3}y]", Minutes, Hours, Days, Years);
    }
}
