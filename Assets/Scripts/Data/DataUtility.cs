using System.Collections.Generic;
using System.IO;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
#endif
using UnityEngine;

public static class DataUtility
{
#if UNITY_EDITOR

    public static T CreateScriptableObject<T>(string name) where T : ScriptableObject
    {
        T s = ImportDataSingle<T>(name);

        if (s != null) { return s; }

        return CreateScriptableObject(name, typeof(T).Name) as T;
    }

    public static ScriptableObject CreateScriptableObject(string name, string typeName)
    {
        ScriptableObject s = ScriptableObject.CreateInstance(typeName);
        string dataFolder = typeName.Replace("GDE", "").Replace("Data", "");
        AssetDatabase.CreateAsset(s, string.Format("Assets/Resources_moved/Data/{0}/{1}.asset", dataFolder, name));
        AssetDatabase.SaveAssets();

        SetAddressableGroup(s, "Default Local Group", dataFolder.ToLower());

        return s;
    }

    public static ScriptableObject Clone(ScriptableObject prevObj, string name)
    {
        //Object prefabRoot = PrefabUtility.GetCorrespondingObjectFromSource(prevObj);
        Object newObj = Object.Instantiate(prevObj);
        ScriptableObject s = newObj as ScriptableObject;
        //Debug.LogError(s);
        s.name = name;
        System.Type t = s.GetType();
        string dataFolder = t.Name.Replace("GDE", "").Replace("Data", "");
        AssetDatabase.CreateAsset(s, string.Format("Assets/Resources_moved/Data/{0}/{1}.asset", dataFolder, name));
        AssetDatabase.SaveAssets();

        SetAddressableGroup(s, "Default Local Group", dataFolder.ToLower());

        return s;
    }

    public static void ReplaceTextInFields(ScriptableObject obj, string originalText, string newText)
    {
        FieldInfo[] fields = obj.GetType().GetFields();

        for (int i = 0; i < fields.Length; i++)
        {
            if (fields[i].FieldType.Equals(typeof(string)))
            {
                string v = fields[i].GetValue(obj) as string;

                if (string.IsNullOrEmpty(v) || !v.Contains(originalText)) { continue; }
                fields[i].SetValue(obj, (string)v.Replace(originalText, newText));
            }
            else if (fields[i].FieldType.Equals(typeof(List<string>)))
            {
                List<string> vList = fields[i].GetValue(obj) as List<string>;

                for (int j = 0; j < vList.Count; j++)
                {
                    string v = vList[j];
                    if (string.IsNullOrEmpty(v) || !v.Contains(originalText)) { continue; }
                    vList[j] = v.Replace(originalText, newText);
                }
            }
        }

        EditorUtility.SetDirty(obj);
    }


    public static void SetAddressableGroup(this Object obj, string groupName, string labelName)
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        if (settings)
        {
            var group = settings.FindGroup(groupName);
            if (!group)
                group = settings.CreateGroup(groupName, false, false, true, null, typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));

            var assetpath = AssetDatabase.GetAssetPath(obj);
            var guid = AssetDatabase.AssetPathToGUID(assetpath);

            var e = settings.CreateOrMoveEntry(guid, group, false, false);
            var entriesAdded = new List<AddressableAssetEntry> { e };

            e.SetLabel(labelName, true);
            group.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, false, true);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true, false);
        }
    }

    public static Sprite[] ImportAnimationSpriteSheetSprites(string entity)
    {
        string path = "Assets/Resources_moved/Animation/Entities/" + entity + "/Sprites/" + entity + "_animations.png";
        Object[] sheet = AssetDatabase.LoadAllAssetsAtPath(path);


        if (sheet == null)
        {
            Debug.LogError("No sprite sheet found for: " + path);
            return null;
        }

        List<Sprite> sheetSprites = new List<Sprite>();

        for (int i = 0; i < sheet.Length; i++)
        {
            Sprite s = sheet[i] as Sprite;
            if (s == null) { continue; }

            sheetSprites.Add(s);
        }

        return sheetSprites.ToArray();
    }

    public static T ImportDataSingle<T>(string name) where T : ScriptableObject
    {
        if (string.IsNullOrEmpty(name)) { return null; }
        string dataFolder = typeof(T).Name.Replace("GDE", "").Replace("Data", "");
        string path = "Assets/Resources_moved/Data/" + dataFolder + "\\" + name + ".asset";
        return AssetDatabase.LoadAssetAtPath<T>(path);
    }

    public static void ImportData<T>(List<T> l, string name, System.Action<T> onAdded = null) where T : ScriptableObject
    {
        l?.Clear();

        string[] files = Directory.GetFiles(Application.dataPath + "/Resources_moved/Data/" + name);

        for (int i = 0; files != null && i < files.Length; i++)
        {
            string path = "Assets" + files[i].Replace(Application.dataPath, "");
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);

            if (asset == null) { continue; }

            onAdded?.Invoke(asset);
            l?.Add(asset);
        }
    }

    public static void ImportAllScriptables(string name, System.Action<Scriptable> onAdded)
    {
        string dir = Application.dataPath + "/Resources_moved/Data/" + name;

        if (!Directory.Exists(dir)) { return; }

        string[] files = Directory.GetFiles(dir);

        for (int i = 0; files != null && i < files.Length; i++)
        {
            string path = "Assets" + files[i].Replace(Application.dataPath, "");
            Scriptable asset = AssetDatabase.LoadAssetAtPath<Scriptable>(path);

            if (asset == null) { continue; }

            onAdded?.Invoke(asset);
        }

        //Debug.Log(name + " loaded.");
    }

    public static T LoadEditorArtAsset<T>(string name) where T : Object
    {
        string path = "Assets/Editor/Art/" + name + ".png";

        Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);

        return asset as T;
    }

#endif
}