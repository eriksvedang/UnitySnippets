using UnityEngine;
using UnityEditor;
using System.Collections;

public class Something : ScriptableObject 
{
	public string title;
	public int age;
}

/// <summary>
/// Creates an asset that can be used as data for game objects and prefabs.
/// This asset file can also be edited with the inspector, etc.
/// </summary>
public class ScriptableObjectCreator 
{	
	[MenuItem("ScriptableObjectCreator/Create Something")]
    public static void CreateMyAsset()
    {
        Something s = ScriptableObject.CreateInstance<Something>();
        AssetDatabase.CreateAsset(s, "Assets/a new something.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = s;
    }
}
