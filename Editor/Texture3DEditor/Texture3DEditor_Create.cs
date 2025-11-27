using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace BDXK
{
    public partial class Texture3DEditor
    {
        [FoldoutGroup("Create")] [SerializeField]
        string rootPath;
        [FoldoutGroup("Create")] [SerializeField]
        Vector3Int dimension;
        [FoldoutGroup("Create")] [SerializeField]
        FilterMode filterMode = FilterMode.Bilinear;
        [FoldoutGroup("Create")] [SerializeField]
        TextureWrapMode wrapMode = TextureWrapMode.Clamp;
        [FoldoutGroup("Create", expanded: false)] [Button("Invoke")]
        void Create()
        {
            Texture3D texture = new Texture3D(dimension.x, dimension.y, dimension.z, GraphicsFormat.R8_UNorm, TextureCreationFlags.None);
            AssetDatabase.CreateAsset(texture, rootPath + "/New Texture3D.asset");
            canvas = texture;
        }
    }
}
