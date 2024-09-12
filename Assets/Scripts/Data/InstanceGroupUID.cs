using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct InstanceGroupUID : IEquatable<InstanceGroupUID>
{
    private readonly uint _uid;

    public bool IsNULL { get { return _uid == 0; } }
    public static readonly InstanceGroupUID NULL = new InstanceGroupUID();

    public InstanceGroupUID(uint uid)
    {
        _uid = uid;
    }

    public static implicit operator InstanceGroupUID(uint value)
    {
        return new InstanceGroupUID(value);
    }

    public static implicit operator uint(InstanceGroupUID value)
    {
        return value._uid;
    }

    public static uint UIDS = 0; // 0 = NULL
    public static InstanceGroupUID Next() { return ++UIDS; }

    public bool Equals(InstanceGroupUID other)
    {
        return this._uid == other._uid;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is InstanceGroupUID))
            return false;

        InstanceGroupUID other = (InstanceGroupUID)obj;
        return _uid == other._uid;
    }

    public override int GetHashCode()
    {
        return _uid.GetHashCode();
    }

    public static bool operator ==(InstanceGroupUID left, InstanceGroupUID right)
    {
        return left._uid == right._uid;
    }

    public static bool operator !=(InstanceGroupUID left, InstanceGroupUID right)
    {
        return left._uid != right._uid;
    }

    public override string ToString()
    {
        return string.Format("[{0}]", _uid);
    }
}