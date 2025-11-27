using UnityEditor;
using UnityEngine;

public static class ProceduralTextureEditor
{
    [MenuItem("GameObject/Effects/Procedural Texture")]
    public static void CreateProceduralTexture()
    {
        GameObject root = new GameObject("RenderFeatureSystem");
        root.AddComponent<RenderFeatureSystem>();
        GameObject gameObject = new GameObject("ProceduralTexture");
        gameObject.transform.SetParent(root.transform);
        ProceduralTexture proceduralTexture = gameObject.AddComponent<ProceduralTexture>();
        CameraTextureCanvas cameraTextureCanvas = gameObject.AddComponent<CameraTextureCanvas>();
        proceduralTexture.Canvas = cameraTextureCanvas;
        EditorUtility.SetDirty(root);
    }
}
