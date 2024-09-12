using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimulationAction
{
    [System.NonSerialized]
    public bool Expanded;

    [System.NonSerialized]
    public bool Enabled = true;

    //[System.NonSerialized]
    public bool DebugBreakpoint;

    public virtual string GetDisplayName()
    {
        string display = "";

        //if (!string.IsNullOrEmpty(ActionID))
        //{
        //    display += "\"";
        //    display += ActionID;
        //    display += "\" ";
        //}

        display += GetType().Name.Replace("SimAction", "");

        return display;
    }

    //public static HashSet<string> CanHaveChildrenActions = new HashSet<string>() {
    //    typeof(SimulateInternalAction).Name
    //};

    public static readonly Dictionary<string, System.Func<SimulationAction>> Actions = new Dictionary<string, System.Func<SimulationAction>>
    {
        //{ typeof(SimulateInternalAction).Name, () => new SimulateInternalAction() },
        { typeof(ActivateStateSimAction).Name, () => new ActivateStateSimAction() },
        { typeof(MoveInstanceSimAction).Name, () => new MoveInstanceSimAction() },
        { typeof(SetLocationSimAction).Name, () => new SetLocationSimAction() },
        { typeof(DuplicateInstanceSimAction).Name, () => new DuplicateInstanceSimAction() },
        { typeof(SpawnInstanceFromTagSimAction).Name, () => new SpawnInstanceFromTagSimAction() },
        { typeof(SpawnInstanceFromTagObjectSimAction).Name, () => new SpawnInstanceFromTagObjectSimAction() },
        { typeof(DisposeSimAction).Name, () => new DisposeSimAction() },
        { typeof(ClearSimAction).Name, () => new ClearSimAction() },
    };

    public static SimulationAction GetNewAction(string typeName)
    {
        System.Func<SimulationAction> actionFactory;

        if (Actions.TryGetValue(typeName, out actionFactory))
        {
            return actionFactory();
        }

        return null;
    }

#if ODD_REALM_APP
    public virtual void Simulate(ISimulationManager manager, Location target, ISimulationObject originInstance, SimulationState prevState, SimulationState newState)
    {

    }
#endif
}

[System.Serializable]
public class ActivateStateSimAction : SimulationAction
{
    [Header("State ID - Internal state that is specific to a sim obj. i.e., plant_state_grow")]
    public string StateID = "";

    [Header("Optional ID - Used to further define the intent of a state. i.e., plant_tree_branch")]
    public string OptionID = "";

    [Header("Take our group UID from the origin instance if it isn't 0, create a new one if it is")]
    public bool UseGroupUID = false;

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
    public override void Simulate(ISimulationManager manager,
                                  Location target,
                                  ISimulationObject originInstance,
                                  SimulationState prevState,
                                  SimulationState newState)
    {
        base.Simulate(manager, target, originInstance, prevState, newState);

        InstanceGroupUID groupUID = 0;

        if (UseGroupUID)
        {
            if (originInstance.GroupUID == 0)
            {
                groupUID = InstanceGroupUID.Next();
            }
            else
            {
                groupUID = originInstance.GroupUID;
            }
        }

        originInstance.ActivateState(StateID, target, newState, groupUID, OptionID);
    }
    #endif
}

[System.Serializable]
public class MoveInstanceSimAction : SimulationAction
{
#if ODD_REALM_APP
    public override void Simulate(ISimulationManager manager, Location target, ISimulationObject originInstance, SimulationState prevState, SimulationState newState)
    {
        base.Simulate(manager, target, originInstance, prevState, newState);

        originInstance.MoveTo(target);
    }
#endif
}

[System.Serializable]
public class ClearSimAction : SimulationAction
{
    public override string GetDisplayName()
    {
        string display = base.GetDisplayName();

        display += $" ({Clear.Layer})";

        return display;
    }

    public BlockClear Clear;

#if ODD_REALM_APP
    public override void Simulate(ISimulationManager manager, Location target, ISimulationObject originInstance, SimulationState prevState, SimulationState newState)
    {
        base.Simulate(manager, target, originInstance, prevState, newState);

        Location.ClearLocation(target.LocationUID, Clear);
    }
#endif
}

[System.Serializable]
public class SetLocationSimAction : SimulationAction
{
#if ODD_REALM_APP
    public override void Simulate(ISimulationManager manager, Location target, ISimulationObject originInstance, SimulationState prevState, SimulationState newState)
    {
        base.Simulate(manager, target, originInstance, prevState, newState);

        originInstance.MoveTo(manager.GetSimLocation(target.LocationUID));
    }
#endif
}

[System.Serializable]
public class DuplicateInstanceSimAction : SimulationAction
{
    [Header("Take our group UID from the origin instance if it isn't 0, create a new one if it is")]
    public bool UseGroupUID = false;

    public override string GetDisplayName()
    {
        string display = base.GetDisplayName();

        if (UseGroupUID)
        {
            display += "[GROUP]";
        }

        display += " (Origin.TagObject.Key)";

        return display;
    }

#if ODD_REALM_APP
    public override void Simulate(ISimulationManager manager, Location target, ISimulationObject originInstance, SimulationState prevState, SimulationState newState)
    {
        base.Simulate(manager, target, originInstance, prevState, newState);

        InstanceGroupUID groupUID = originInstance.GroupUID;

        if (UseGroupUID && groupUID == 0)
        {
            groupUID = InstanceGroupUID.Next();
            originInstance.SetGroup(groupUID);
        }

        manager.SpawnSimTagObject(originInstance.TagObjectData, target.LocationUID, newState, groupUID);
    }
#endif
}

[System.Serializable]
public class SpawnInstanceFromTagSimAction : SimulationAction
{
    [Header("Spawn new instances from a tag")]
    public string SpawnTag;

    [Header("Take our group UID from the origin instance if it isn't 0, create a new one if it is")]
    public bool UseGroupUID = false;

    public override string GetDisplayName()
    {
        string display = base.GetDisplayName();

        if (UseGroupUID)
        {
            display += "[GROUP]";
        }

        if (!string.IsNullOrEmpty(SpawnTag))
        {
            display += " (";
            display += SpawnTag;
            display += ")";
        }
        else
        {
            display += " (CANNOT BE EMPTY!)";
        }

        return display;
    }

#if ODD_REALM_APP
    public override void Simulate(ISimulationManager manager, Location target, ISimulationObject originInstance, SimulationState prevState, SimulationState newState)
    {
        base.Simulate(manager, target, originInstance, prevState, newState);

        if (string.IsNullOrEmpty(SpawnTag))
        {
            Debug.LogError("SpawnTag cannot be empty!");
            return;
        }

        InstanceGroupUID groupUID = originInstance.GroupUID;

        if (UseGroupUID && groupUID == 0)
        {
            groupUID = InstanceGroupUID.Next();
            originInstance.SetGroup(groupUID);
        }

        List<ITagObject> tagObjects = DataManager.GetTagObjectsByTag(SpawnTag);

        for (int i = 0; i < tagObjects.Count; i++)
        {
            manager.SpawnSimTagObject(tagObjects[i], target.LocationUID, newState, groupUID);
        }
    }
    #endif
}

[System.Serializable]
public class SpawnInstanceFromTagObjectSimAction : SimulationAction
{
    [Header("Spawn new instances with tag object key")]
    public string TagObjectKey;

    [Header("Take our group UID from the origin instance if it isn't 0, create a new one if it is")]
    public bool UseGroupUID = false;

    public override string GetDisplayName()
    {
        string display = base.GetDisplayName();

        if (UseGroupUID)
        {
            display += "[GROUP]";
        }

        if (!string.IsNullOrEmpty(TagObjectKey))
        {
            display += " (";
            display += TagObjectKey;
            display += ")";
        }
        else
        {
            display += " (Origin.TagObject.Key)";
        }

        return display;
    }

#if ODD_REALM_APP
    public override void Simulate(ISimulationManager manager, Location target, ISimulationObject originInstance, SimulationState prevState, SimulationState newState)
    {
        base.Simulate(manager, target, originInstance, prevState, newState);

        if (string.IsNullOrEmpty(TagObjectKey))
        {
            Debug.LogError("SpawnTagObject cannot be empty!");
            return;
        }

        InstanceGroupUID groupUID = originInstance.GroupUID;

        if (UseGroupUID && groupUID == 0)
        {
            groupUID = InstanceGroupUID.Next();
            originInstance.SetGroup(groupUID);
        }

        ITagObject tagObject = DataManager.GetTagObject(TagObjectKey);
        manager.SpawnSimTagObject(tagObject, target.LocationUID, newState, groupUID);
    }
    #endif
}

[System.Serializable]
public class DisposeSimAction : SimulationAction
{
#if ODD_REALM_APP
    public override void Simulate(ISimulationManager manager, Location target, ISimulationObject originInstance, SimulationState prevState, SimulationState newState)
    {
        base.Simulate(manager, target, originInstance, prevState, newState);

        // Dispose of instance
        originInstance.Dispose();
    }
    #endif
}