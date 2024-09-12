using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// This gets generated on data load
public struct TagUID : IEquatable<TagUID>
{
    private readonly uint _uid;

    public bool IsNULL { get { return _uid == 0; } }
    public static readonly TagUID NULL = new TagUID();

    public TagUID(uint uid)
    {
        _uid = uid;
    }

    public static implicit operator TagUID(uint value)
    {
        return new TagUID(value);
    }

    public static implicit operator uint(TagUID value)
    {
        return value._uid;
    }

    public static implicit operator int(TagUID value)
    {
        return (int)value._uid;
    }

    public static uint UIDS = 2; // 0 = null, 1 = empty, 2 = saved, 3+ = tag objs
    public static TagUID Next() { return ++UIDS; }

    public bool Equals(TagUID other)
    {
        return this._uid == other._uid;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is TagUID))
            return false;

        TagUID other = (TagUID)obj;
        return _uid == other._uid;
    }

    public override int GetHashCode()
    {
        return _uid.GetHashCode();
    }

    public static bool operator ==(TagUID left, TagUID right)
    {
        return left._uid == right._uid;
    }

    public static bool operator !=(TagUID left, TagUID right)
    {
        return left._uid != right._uid;
    }

    public override string ToString()
    {
        return string.Format("[{0}]", _uid);
    }
}