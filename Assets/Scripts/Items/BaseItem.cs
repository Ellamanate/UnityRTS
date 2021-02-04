using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public abstract class BaseItem : ScriptableObject
{
    public Sprite Icon;

    public virtual void Collect(Unit _owner, ItemPrefab _prefab) 
    {
        Activate(_owner, _prefab);
        Events.OnItemCollect.Publish(this);
    }

    public virtual void Activate(Unit _owner, ItemPrefab _prefab) { }

    protected static void SaveInstance(ScriptableObject _obj, string _name)
    {
        string _uniqueFileName = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/Items/" + _name + ".asset");
        AssetDatabase.CreateAsset(_obj, _uniqueFileName);
        AssetDatabase.Refresh();
    }
}