using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimOptions
{
    public string OptionID;
    public string TagObjectID;

    [System.NonSerialized]
    public ITagObject TagObj;
}

[System.Serializable]
public class SimulationCondition
{
    [System.NonSerialized]
    public bool Expanded;

    [System.NonSerialized]
    public bool Enabled = true;

    [Header("Whether the outcome must be true or false")]
    public bool Outcome = true;

    [System.NonSerialized]
    public bool FORCE_PASS;
    [System.NonSerialized]
    public bool TEST;
    [System.NonSerialized]
    public bool DebugBreakpoint;

    // Called when game data loads
    public virtual void Init()
    {

    }

    public virtual string GetDisplayName()
    {
        string display = "";

        display += GetType().Name.Replace("SimCondition", "");

        if (!Outcome)
        {
            display += " [MUST FAIL]";
        }

        return display;
    }

#if ODD_REALM_APP

    public virtual bool IsConditionMet(ISimulationManager manager,
                                       Location target,
                                       ISimulationObject origin,
                                       SimulationState prevState,
                                       SimulationState newState)
    {
        if (TEST)
        {
            target.Point.RenderLineToPoint(origin.Point, Color.red, 0.1f);
            Debug.LogError(target);
        }

        return FORCE_PASS;
    }

    public static bool IsSimConditionMet(ISimulationManager manager,
                                         ISimulationObject simObj,
                                         bool requireAll,
                                         Location target,
                                         SimulationCondition[] conditions,
                                         SimulationState prevState,
                                         SimulationState newState)
    {
        if (target.IsNULL) { return false; }

        int conditionCount = conditions.Length;

        if (conditionCount == 0) { return true; }

        int req = requireAll ? conditionCount : 1;
        int pass = 0;

#if DEV_TESTING
        if (Master.IsDebugPoint(target))
        {
            // Breakpoint.
        }
#endif

        for (int i = 0; i < conditionCount; i++)
        {
            SimulationCondition condition = conditions[i];

            if (!condition.Enabled) { continue; }

#if DEV_TESTING
            if (condition.DebugBreakpoint)
            {
                // Breakpoint.
            }
#endif
            bool conditionOutcome = condition.IsConditionMet(manager, target, simObj, prevState, newState) == condition.Outcome;

            pass += conditionOutcome ? 1 : 0;

            if (!requireAll && pass >= req) return true;
            if (requireAll && !conditionOutcome) return false;
        }

        return pass >= req;
    }
#endif

    public static readonly Dictionary<string, System.Func<SimulationCondition>> Conditions = new Dictionary<string, System.Func<SimulationCondition>>
    {
        { typeof(HasSimStateSimCondition).Name, () => new HasSimStateSimCondition() },
        { typeof(CanPathToSimCondition).Name, () => new CanPathToSimCondition() },
        { typeof(IsObstructionSimCondition).Name, () => new IsObstructionSimCondition() },
        { typeof(HasSkylightSimCondition).Name, () => new HasSkylightSimCondition() },
        { typeof(CanSupportWeightSimCondition).Name, () => new CanSupportWeightSimCondition() },
        { typeof(RandomChanceSimCondition).Name, () => new RandomChanceSimCondition() },
        { typeof(HasNeighborSimCondition).Name, () => new HasNeighborSimCondition() },
        { typeof(HasInstanceSimCondition).Name, () => new HasInstanceSimCondition() },
        { typeof(MaxGroupRadiusSimCondition).Name, () => new MaxGroupRadiusSimCondition() },
        { typeof(InGroupVerticalBoundsSimCondition).Name, () => new InGroupVerticalBoundsSimCondition() },
        { typeof(HasBlockLayerSimCondition).Name, () => new HasBlockLayerSimCondition() },
    };

    public static SimulationCondition GetNewCondition(string typeName)
    {
        System.Func<SimulationCondition> conditionFactory;

        if (Conditions.TryGetValue(typeName, out conditionFactory))
        {
            return conditionFactory();
        }

        return null;
    }
}

[System.Serializable]
public class MaxGroupRadiusSimCondition : SimulationCondition
{
    public float MaxRadiusPercent = 1f;

#if ODD_REALM_APP

    public override bool IsConditionMet(ISimulationManager manager, Location target, ISimulationObject origin, SimulationState prevState, SimulationState newState)
    {
        bool outcome = base.IsConditionMet(manager, target, origin, prevState, newState);

        if (origin.GroupUID == 0)
        {
            return outcome;
        }

        UintArray group = manager.GetSimObjectsByGroup(origin.GroupUID);

        if (group == null || group.Count <= 1) { return outcome; }

        ISimulationObject centerSimObj = manager.GetSimObjectByUID(group[0]);

        if (centerSimObj.IsNULL) { return outcome; }

        Location centerSimLocation = centerSimObj.Location;

        if (centerSimLocation.IsNULL) { return outcome; }

        int count = Mathf.Min(100, group.Count);
        int minX = centerSimLocation.x;
        int maxX = centerSimLocation.x;
        int minY = centerSimLocation.y;
        int maxY = centerSimLocation.y;

        for (int i = 1; i < count; i++)
        {
            ISimulationObject simObj = manager.GetSimObjectByUID(group[i]);

            if (simObj.IsNULL) { continue; }

            Location l = simObj.Location;

            if (l.IsNULL) { continue; }

            if (l.x < minX) minX = l.x;
            if (l.x > maxX) maxX = l.x;

            if (l.y < minY) minY = l.y;
            if (l.y > maxY) maxY = l.y;
        }

        int width = maxX - minX;
        int height = maxY - minY;

        int radiusX = width / 2;
        int radiusY = height / 2;
        float maxRadius = Mathf.Max(1, Mathf.Max(radiusX, radiusY));

        float distToCenter = BlockPoint.Distance(new BlockPoint(target.Point.x, target.Point.y, 0),
                                                 new BlockPoint(centerSimLocation.Point.x, centerSimLocation.Point.y, 0));

        float n = distToCenter / maxRadius;

        outcome = n <= MaxRadiusPercent;

        return outcome;
    }
#endif
}

[System.Serializable]
public class InGroupVerticalBoundsSimCondition : SimulationCondition
{
    public int TopBuffer = 0;
    public int BottomBuffer = 0;

#if ODD_REALM_APP

    public override bool IsConditionMet(ISimulationManager manager, Location target, ISimulationObject origin, SimulationState prevState, SimulationState newState)
    {
        bool outcome = base.IsConditionMet(manager, target, origin, prevState, newState);

        if (origin.GroupUID == 0)
        {
            return outcome;
        }

        UintArray group = manager.GetSimObjectsByGroup(origin.GroupUID);

        if (group == null || group.Count <= 1) { return outcome; }

        ISimulationObject centerSimObj = manager.GetSimObjectByUID(group[0]);

        if (centerSimObj.IsNULL) { return outcome; }

        Location centerSimLocation = centerSimObj.Location;

        if (centerSimLocation.IsNULL) { return outcome; }

        int minZ = centerSimLocation.z;
        int maxZ = centerSimLocation.z;

        for (int i = 0; i < group.Count; i++)
        {
            ISimulationObject simObj = manager.GetSimObjectByUID(group[i]);

            if (simObj.IsNULL) { continue; }

            Location l = simObj.Location;

            if (l.IsNULL) { continue; }

            if (l.z < minZ) minZ = l.z;
            if (l.z > maxZ) maxZ = l.z;
        }

        if (minZ == maxZ) { return outcome; }

        if ((target.z - minZ) >= BottomBuffer &&
            (maxZ - target.z) >= TopBuffer)
        {
            outcome = true;
        }
        else
        {
            outcome = false;
        }

        return outcome;
    }
#endif
}

[System.Serializable]
public class HasInstanceSimCondition : SimulationCondition
{
    public string TagObjectKey;
    public string StateID;
    public string OptionID;

    public override string GetDisplayName()
    {
        string display = base.GetDisplayName();

        if (!string.IsNullOrEmpty(TagObjectKey))
        {
            display += " (";
            display += TagObjectKey;
            display += ")";
        }
        else if (!string.IsNullOrEmpty(StateID))
        {
            display += " (";
            display += StateID;
            display += ")";

            if (!string.IsNullOrEmpty(OptionID))
            {
                display += " Option:\"";
                display += OptionID;
                display += "\" ";
            }
        }
        else
        {
            display += " (Origin.TagObject.Key)";
        }

        return display;
    }

#if ODD_REALM_APP

    public override bool IsConditionMet(ISimulationManager manager, Location target, ISimulationObject origin, SimulationState prevState, SimulationState newState)
    {
        bool outcome = base.IsConditionMet(manager, target, origin, prevState, newState);

        if (target.packedCounts == 0) { return outcome; }

        string key = TagObjectKey;

        if (string.IsNullOrEmpty(key) && string.IsNullOrEmpty(StateID))
        {
            key = origin.TagObjectData.Key;
        }

        UintArray instances = manager.GetSimObjectsByLocation(target.locationUID);

        for (int i = 0; i < instances.Count; i++)
        {
            ISimulationObject objInstance = manager.GetSimObjectByUID(instances[i]);

            if (!string.IsNullOrEmpty(key) && objInstance.TagObjectData.Key != key) { continue; }
            if (!string.IsNullOrEmpty(StateID) && !objInstance.HasSimState(StateID, target, prevState, newState, OptionID)) { continue; }
            outcome |= true;
            break;
        }

        return outcome;
    }
#endif
}

[System.Serializable]
public class HasNeighborSimCondition : SimulationCondition, ISimulationConditionSource
{
    [Header("These are offset from the target")]
    [SerializeReference]
    public SimPositions NeighborPositions = new SimPositions();

    [SerializeReference]
    public SimulationCondition[] NeighborConditions = new SimulationCondition[] { };

    public SimulationCondition[] GetConditions() { return NeighborConditions; }
    public void SetConditions(SimulationCondition[] conditions) { NeighborConditions = conditions; }

#if ODD_REALM_APP

    public override bool IsConditionMet(ISimulationManager manager, Location target, ISimulationObject origin, SimulationState prevState, SimulationState newState)
    {
        bool outcome = base.IsConditionMet(manager, target, origin, prevState, newState);

        if (NeighborPositions == null || NeighborPositions.Positions.Length == 0) { return outcome; }
        if (NeighborConditions == null || NeighborConditions.Length == 0) { return outcome; }

        BlockPoint targetPoint = target.Point;
        BlockPoint originPoint = origin.Point;

        for (int i = 0; !outcome && i < NeighborPositions.Positions.Length; i++)
        {
            BlockPoint np = targetPoint + new BlockPoint(NeighborPositions.Positions[i]);
            if (np == originPoint) { continue; }
            Location l = manager.GetSimLocation(new LocationUID(np));

            for (int n = 0; !outcome && n < NeighborConditions.Length; n++)
            {
                SimulationCondition neighborCondition = NeighborConditions[n];
                outcome |= neighborCondition.IsConditionMet(manager, l, origin, prevState, newState) == neighborCondition.Outcome;
            }
        }

        return outcome;
    }
#endif
}

[System.Serializable]
public class HasBlockLayerSimCondition : SimulationCondition
{
    public override string GetDisplayName()
    {
        string display = base.GetDisplayName();

        display += $" ({Layer})";

        return display;
    }

    public BlockLayers Layer;

#if ODD_REALM_APP

    public override bool IsConditionMet(ISimulationManager manager, Location target, ISimulationObject origin, SimulationState prevState, SimulationState newState)
    {
        bool outcome = base.IsConditionMet(manager, target, origin, prevState, newState);

        if (target.packedCounts == 0) { return outcome; }

        outcome = Location.HasLayer(target, Layer);

        return outcome;
    }
#endif
}

[System.Serializable]
public class CanPathToSimCondition : SimulationCondition
{
    [Header("i.e., does the origin allow pathing out of itself in the target direction")]
    public bool Entry = true;
    [Header("i.e., does the target allow pathing into itself from the origin direction")]
    public bool Exit = true;

#if ODD_REALM_APP

    public override bool IsConditionMet(ISimulationManager manager, Location target, ISimulationObject origin, SimulationState prevState, SimulationState newState)
    {
        bool outcome = base.IsConditionMet(manager, target, origin, prevState, newState);

        if (!target.isObstruction)
        {
            outcome |= Location.CanPathTo(origin.Location, target, Entry, Exit);
        }

        return outcome;
    }
#endif
}

[System.Serializable]
public class CanSupportWeightSimCondition : SimulationCondition
{
#if ODD_REALM_APP

    public override bool IsConditionMet(ISimulationManager manager, Location target, ISimulationObject origin, SimulationState prevState, SimulationState newState)
    {
        bool outcome = base.IsConditionMet(manager, target, origin, prevState, newState);

        outcome |= target.HasSupport;



        return outcome;
    }
#endif
}

[System.Serializable]
public class IsObstructionSimCondition : SimulationCondition
{
    public override string GetDisplayName()
    {
        string display = base.GetDisplayName();

        return display;
    }

#if ODD_REALM_APP

    public override bool IsConditionMet(ISimulationManager manager, Location target, ISimulationObject origin, SimulationState prevState, SimulationState newState)
    {
        bool outcome = base.IsConditionMet(manager, target, origin, prevState, newState);
        outcome |= target.isObstruction;

        return outcome;
    }
#endif
}

[System.Serializable]
public class HasSkylightSimCondition : SimulationCondition
{
    public int MaxAmount = 7;
    public int MinAmount = 0;

    public override string GetDisplayName()
    {
        string display = base.GetDisplayName();

        return display;
    }

#if ODD_REALM_APP

    public override bool IsConditionMet(ISimulationManager manager, Location target, ISimulationObject origin, SimulationState prevState, SimulationState newState)
    {
        bool outcome = base.IsConditionMet(manager, target, origin, prevState, newState);
        outcome |= (target.Skylight >= MinAmount && target.Skylight <= MaxAmount);

        return outcome;
    }
#endif
}

[System.Serializable]
public class HasSimStateSimCondition : SimulationCondition
{
    public string StateID = "";
    public string OptionID = "";

    public override string GetDisplayName()
    {
        string display = base.GetDisplayName();

        if (!string.IsNullOrEmpty(StateID))
        {
            display += " (";
            display += StateID;
            display += ")";
        }

        if (!string.IsNullOrEmpty(OptionID))
        {
            display += " Option:\"";
            display += OptionID;
            display += "\" ";
        }

        return display;
    }

#if ODD_REALM_APP

    public override bool IsConditionMet(ISimulationManager manager, Location target, ISimulationObject origin, SimulationState prevState, SimulationState newState)
    {
        bool outcome = base.IsConditionMet(manager, target, origin, prevState, newState);

        outcome |= origin.HasSimState(StateID, target, prevState, newState, OptionID);

        return outcome;
    }
#endif
}

[System.Serializable]
public class IsExpiredSimCondition : SimulationCondition
{
#if ODD_REALM_APP

    public override bool IsConditionMet(ISimulationManager manager, Location target, ISimulationObject origin, SimulationState prevState, SimulationState newState)
    {
        bool outcome = base.IsConditionMet(manager, target, origin, prevState, newState);
        //outcome |= origin.IsExpired();

        return outcome;
    }
#endif
}

[System.Serializable]
public class IsMatureSimCondition : SimulationCondition
{
#if ODD_REALM_APP

    public override bool IsConditionMet(ISimulationManager manager, Location target, ISimulationObject origin, SimulationState prevState, SimulationState newState)
    {
        bool outcome = base.IsConditionMet(manager, target, origin, prevState, newState);
        //outcome |= origin.IsMature();

        return outcome;
    }
#endif
}

[System.Serializable]
public class Vec2RelativeSizeInGroupSimCondition : SimulationCondition
{
    [Header("Min/Max Relative Size compared to size of horizontal footprint of group. e.g., If the footprint magnitude is 8 and MaxRelativeSize is 0.8, only accept targets withing 80% of the 8 point magnitude")]
    public float MinRelativeSize;
    public float MaxRelativeSize;
    public bool IncludeInstancesOfSameObjectKey = true;

#if ODD_REALM_APP

    public override bool IsConditionMet(ISimulationManager manager, Location target, ISimulationObject origin, SimulationState prevState, SimulationState newState)
    {
        bool outcome = base.IsConditionMet(manager, target, origin, prevState, newState);

        UintArray instances = manager.GetSimObjectsByGroup(origin.GroupUID);

        // If it's just us, pass
        if (instances.Count <= 1) { return true; }

        Bounds b = new Bounds(origin.Position, Vector3.zero);

        for (int i = 0; i < instances.Count; i++)
        {
            ISimulationObject objInstance = manager.GetSimObjectByUID(instances[i]);

            if (objInstance.IsNULL) { continue; }
            if (!IncludeInstancesOfSameObjectKey && objInstance.TagObjectData.Key != origin.TagObjectData.Key) { continue; }
            Vector3 p = objInstance.Position;
            Vector2 center = new Vector2(p.x, p.y);
            b.Encapsulate(center);
        }

        float dist = Vector2.Distance(b.center, target.Point.ToVector2());
        float magnitude = b.size.magnitude;

        if (dist >= (MinRelativeSize * magnitude) && dist <= (MaxRelativeSize * magnitude))
        {
            outcome = true;
        }

        return outcome;
    }
#endif
}

[System.Serializable]
public class Vec2DistanceFromGroupCentreSimCondition : SimulationCondition
{
    [Header("Max Distance from the centre of the group")]
    public float MaxDistance;

    [Header("Min Distance from the centre of the group")]
    public float MinDistance;

#if ODD_REALM_APP

    public override bool IsConditionMet(ISimulationManager manager, Location target, ISimulationObject origin, SimulationState prevState, SimulationState newState)
    {
        bool outcome = base.IsConditionMet(manager, target, origin, prevState, newState);

        UintArray instances = manager.GetSimObjectsByGroup(origin.GroupUID);

        // If it's just us, pass
        if (instances.Count <= 1) { return true; }

        Vector2 center = Vector2.zero;

        for (int i = 0; i < instances.Count; i++)
        {
            ISimulationObject objInstance = manager.GetSimObjectByUID(instances[i]);

            if (objInstance.IsNULL) { continue; }

            Vector3 p = objInstance.Position;
            center += new Vector2(p.x, p.y);
        }

        center /= instances.Count;

        float dist = Vector2.Distance(center, target.Point.ToVector2());

        if (dist >= MinDistance && dist <= MaxDistance)
        {
            outcome = true;
        }

        return outcome;
    }
#endif
}

[System.Serializable]
public class RandomChanceSimCondition : SimulationCondition
{
    public uint MaxRange;

    public const uint MAX_RANGE = 60 * 24 * 60; // 1 year of minutes

    //private int _threshold;

    public override void Init()
    {
        base.Init();

        //_threshold = Mathf.Max((int)(RandomChance * RandomRange), 1);
    }

    public override string GetDisplayName()
    {
        string display = base.GetDisplayName();

        float probabilityOfWinPerMinute = 1.0f / (float)Mathf.Max(MaxRange, 1);

        int minPerDay = 60 * 24;

        float probabilityOfWinPerDay = 1f - Mathf.Pow((float)(MaxRange - 1) / (float)MaxRange, (float)minPerDay);

        display += $" (per min {probabilityOfWinPerMinute.ToString("0.000%")} - per day {probabilityOfWinPerDay.ToString("0.000%")}";

        return display;
    }

#if ODD_REALM_APP

    public override bool IsConditionMet(ISimulationManager manager, Location target, ISimulationObject origin, SimulationState prevState, SimulationState newState)
    {
        bool outcome = base.IsConditionMet(manager, target, origin, prevState, newState);

        // 80,000s
        // 0.08
        // 1 / 1,000,000s
        int roll = (int)TinyBeast.Random.FloatRange(0, MaxRange);
        SimTime difference = (newState.SimAge - prevState.SimAge);
        outcome |= (roll) < 1 + difference;

        return outcome;
    }
#endif
}