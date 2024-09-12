using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class DataEditSimPage : DataEditPage
{
    
    public override string PageName { get { return "Sim"; } }

    private Vector2 _scriptScroll;
    //private InstanceSimulation _selectedSim;
    //private InstanceSimulation[] Copy;

    public override void OnSelectionChange()
    {
        //_selectedSim = null;
        base.OnSelectionChange();
    }

    private List<Scriptable> _sims = new List<Scriptable>();
    private Vector2 _simScroll;
    //private string _surrogateToCreate;
    //private string _surrogateTagKeyFormat;

    protected override void LoadAllData()
    {
        base.LoadAllData();
    }

    //public override void RenderGUI()
    //{
    //    base.RenderGUI();
    //    //bool addSurrogate = false;

    //    _sims = GetDataByType<GDESimulationData>();

    //    if (_sims == null) { return; }

    //    //if (Selection.objects.Length > 0)
    //    {
    //        _scriptScroll = GUILayout.BeginScrollView(_scriptScroll);

    //        _chooseSimFromList = TOGGLE(_chooseSimFromList, "Show Sim Data");

    //        if (_chooseSimFromList)
    //        {
    //            if (_sims.Count == 0)
    //            {
    //                //DataUtility.ImportData<GDESimulationData>(_sims, "simulation", (GDESimulationData data) => { });
    //            }
    //            else
    //            {
    //                _simScroll = GUILayout.BeginScrollView(_simScroll, GUILayout.Height(100));

    //                for (int n = 0; n < _sims.Count; n++)
    //                {
    //                    BTN(_sims[n].Key, _simToAddToSelections == _sims[n].Key ? Color.white : Color.gray, () => {
    //                        _simToAddToSelections = _sims[n].Key;
    //                    });
    //                }

    //                GUILayout.EndScrollView();
    //            }
    //        }
    //        else
    //        {
    //            //_sims.Clear();
    //        }

    //        //if (!string.IsNullOrEmpty(_simToAddToSelections) && Selection.objects != null)
    //        //{
    //        //    BTN("Add: " + _simToAddToSelections + " to " + Selection.objects.Length + " scripts", Color.magenta, ()=> {
    //        //        addSelectedSimToSelections = true;
    //        //        _chooseSimFromList = false;
    //        //    });

    //        //    BTN("Add If None Exists: " + _simToAddToSelections + " to " + Selection.objects.Length + " scripts", Color.magenta, () => {
    //        //        addSelectedSimToSelections = true;
    //        //        _chooseSimFromList = false;
    //        //        addSelectedSimToSelectionsIfNoneExists = true;
    //        //    });
    //        //}

    //        for (int i = 0; i < _simulatedScripts.Count; i++)
    //        {
    //            bool isSelected = IS_SELECTED(_simulatedScripts[i]);

    //            BTN(_simulatedScripts[i].Key, isSelected ? ColorUtility.selectedGold : Color.white, () =>
    //            {
    //                if (isSelected)
    //                {
    //                    DESELECT(_simulatedScripts[i]);
    //                }
    //                else
    //                {
    //                    SELECT(_simulatedScripts[i]);
    //                }
    //            });

    //            if (!isSelected) 
    //            {
    //                continue;
    //            }

    //        }

    //        GUILayout.EndScrollView();
    //    }

    //    if (addSelectedSimToSelections)
    //    {
    //        _simToAddToSelections = "";
    //    }
    //}
}
