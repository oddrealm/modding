using System;

public struct TooltipUID : IEquatable<TooltipUID>
{
    private uint _uid;

    public bool IsNULL { get { return _uid == 0; } }
    public static readonly TooltipUID NULL = new TooltipUID();

    public static implicit operator TooltipUID(uint value)
    {
        return new TooltipUID { _uid = value };
    }

    public static implicit operator uint(TooltipUID value)
    {
        return value._uid;
    }

    public static uint UIDS;
    public static TooltipUID Next() { return ++UIDS; }

    public bool Equals(TooltipUID other)
    {
        return this._uid == other._uid;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is TooltipUID))
            return false;

        TooltipUID other = (TooltipUID)obj;
        return _uid == other._uid;
    }

    public override int GetHashCode()
    {
        return _uid.GetHashCode();
    }

    public static bool operator ==(TooltipUID left, TooltipUID right)
    {
        return left._uid == right._uid;
    }

    public static bool operator !=(TooltipUID left, TooltipUID right)
    {
        return left._uid != right._uid;
    }

    public override string ToString()
    {
        return string.Format("[{0}]", _uid);
    }
}