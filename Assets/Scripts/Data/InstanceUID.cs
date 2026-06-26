using System;

public struct InstanceUID : IEquatable<InstanceUID>
{
    private readonly uint _uid;

    public readonly bool IsNULL { get { return _uid == NULL; } }

    public static readonly InstanceUID NULL = 0;

    public InstanceUID(uint uid)
    {
        _uid = uid;
    }

    public static implicit operator InstanceUID(uint value)
    {
        return new InstanceUID(value);
    }

    public static implicit operator InstanceUID(int value)
    {
        return new InstanceUID((uint)value);
    }

    public static implicit operator uint(InstanceUID value)
    {
        return value._uid;
    }

    public static implicit operator int(InstanceUID value)
    {
        return (int)value._uid;
    }

#if ODD_REALM_APP
    public static void Init(uint uidStart)
    {
        UIDS = uidStart;
    }

    public static uint UIDS;
    public static InstanceUID Next()
    {
        // #if DEV_TESTING
        //         if (UIDS == 0)
        //         {
        //             UnityEngine.Debug.LogError("UIDs have not been initialized!");
        //         }
        // #endif
        return ++UIDS;
    }
#endif

    public readonly bool Equals(InstanceUID other)
    {
        return this._uid == other._uid;
    }

    public override readonly bool Equals(object obj)
    {
        if (!(obj is InstanceUID))
            return false;

        InstanceUID other = (InstanceUID)obj;
        return _uid == other._uid;
    }

    public override readonly int GetHashCode()
    {
        return _uid.GetHashCode();
    }

    public static bool operator ==(InstanceUID left, InstanceUID right)
    {
        return left._uid == right._uid;
    }

    public static bool operator !=(InstanceUID left, InstanceUID right)
    {
        return left._uid != right._uid;
    }

    public override readonly string ToString()
    {
        return $"[{_uid}]";
    }
}
