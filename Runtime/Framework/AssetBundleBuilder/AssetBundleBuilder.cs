using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class AssetBundleBuilder : ScriptableObject
{
#if UNITY_EDITOR
    [Button]
    public void Build()
    {
        string[] assetGuids = AssetDatabase.FindAssets("", assetBundlePaths);
        foreach (string assetPath in assetGuids.Select(guid => AssetDatabase.GUIDToAssetPath(guid)))
        {
            if (Path.HasExtension(assetPath) == false)
                continue;

            string assetBundleName = assetPath.Substring(0, assetPath.LastIndexOf("/", StringComparison.Ordinal)).ToLower();
            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
            if (importer.assetBundleName != assetBundleName)
            {
                importer.assetBundleName = assetBundleName;
                importer.SaveAndReimport();
            }
        }

        BuildPipeline.BuildAssetBundles(buildDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

        foreach (string filePath in Directory.GetFiles(buildDirectory, "*", SearchOption.AllDirectories))
        {
            if (Path.HasExtension(filePath))
                continue;

            UploadAssetBundle(filePath);
        }
    }
#endif

    public abstract List<AssetBundle> LoadAssetBundle(string targetAssetBundleName);

    protected string BuildDirectory => buildDirectory;
    protected abstract void UploadAssetBundle(string filePath);
    
    [SerializeField] string[] assetBundlePaths;
    [SerializeField] string buildDirectory;
}
