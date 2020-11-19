#if (UNITY_EDITOR) 

using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateStarDataList
{
    [MenuItem("Assets/Create/Star Data List")]
    public static StarDataList Create()
    {
        StarDataList asset = ScriptableObject.CreateInstance<StarDataList>();

        AssetDatabase.CreateAsset(asset, "Assets/StarDataList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}

#endif