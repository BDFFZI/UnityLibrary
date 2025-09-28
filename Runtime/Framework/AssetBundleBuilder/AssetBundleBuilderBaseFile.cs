using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetBundleBuilderBaseFile", menuName = "AssetBundleBuilder/BaseFile")]
public class AssetBundleBuilderBaseFile : AssetBundleBuilder
{
    public override List<AssetBundle> LoadAssetBundle(string targetAssetBundleName)
    {
        //获取清单信息
        AssetBundle manifestAssetBundle = AssetBundle.LoadFromFile(storageDirectory + "/" + Path.GetFileName(BuildDirectory));
        AssetBundleManifest manifest = manifestAssetBundle.LoadAllAssets<AssetBundleManifest>()[0];

        //获取所有需加载的资源包名称
        List<string> assetBundleNames = new List<string>(manifest.GetAllDependencies(targetAssetBundleName));
        assetBundleNames.Add(targetAssetBundleName);
        // List<string> assetBundleNames = new List<string>() { targetAssetBundleName };
        // for (int index = 0; index < assetBundleNames.Count; index++)
        // {
        //     string assetBundleName = assetBundleNames[index];
        //     assetBundleNames.AddRange(manifest.GetDirectDependencies(assetBundleName));
        // }
        // assetBundleNames.Reverse();
        // assetBundleNames = assetBundleNames.Distinct().ToList();

        //加载所有资源包
        List<AssetBundle> assetBundles = new List<AssetBundle>();
        foreach (string assetBundleName in assetBundleNames)
        {
            string assetBundlePath = storageDirectory + "/" + assetBundleName.Replace('/', '-');
            assetBundles.Add(AssetBundle.LoadFromFile(assetBundlePath));
        }

        manifestAssetBundle.Unload(true);

        return assetBundles;
    }

    protected override void UploadAssetBundle(string filePath)
    {
        string assetBundleName = Path.GetRelativePath(BuildDirectory, filePath);
        string assetBundlePath = storageDirectory + "/" + assetBundleName.Replace('\\', '-');

        Directory.CreateDirectory(Path.GetDirectoryName(assetBundlePath)!);
        File.Copy(filePath, assetBundlePath, true);
        Debug.Log("上传资源包：" + assetBundleName);

        File.Delete(filePath);
    }


    [SerializeField] string storageDirectory;
}
