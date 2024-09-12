using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct InputUID : IEquatable<InputUID>
{
    private uint _uid;

    public bool IsNULL { get { return _uid == 0; } }
    public static readonly InputUID NULL = new InputUID();

    public static implicit operator InputUID(uint value)
    {
        return new InputUID { _uid = value };
    }

    public static implicit operator uint(InputUID value)
    {
        return value._uid;
    }

    public static uint UIDS;
    public static InputUID Next() { return ++UIDS; }

    public bool Equals(InputUID other)
    {
        return this._uid == other._uid;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is InputUID))
            return false;

        InputUID other = (InputUID)obj;
        return _uid == other._uid;
    }

    public override int GetHashCode()
    {
        return _uid.GetHashCode();
    }

    public static bool operator ==(InputUID left, InputUID right)
    {
        return left._uid == right._uid;
    }

    public static bool operator !=(InputUID left, InputUID right)
    {
        return left._uid != right._uid;
    }

    public override string ToString()
    {
        return string.Format("[{0}]", _uid);
    }
}