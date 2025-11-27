using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class AssetTextureCanvas : MonoBehaviour, IProceduralTextureCanvas
{
    public RTHandle Texture { get; private set; }
    public string TextureName => renderTexture.name;
    public RenderTextureDescriptor TextureDescriptor { get; private set; }
    public FilterMode TextureFilterMode { get; private set; }
    public TextureWrapMode TextureWrapMode { get; private set; }

    public bool RequestTexture(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        return Texture != null;
    }

    [SerializeField] RenderTexture renderTexture;

    void OnEnable()
    {
        if (renderTexture == null)
            return;

        Texture = RTHandles.Alloc(renderTexture); //这种方式创建的RTHandle无需销毁
        TextureDescriptor = renderTexture.descriptor;
        TextureFilterMode = renderTexture.filterMode;
        TextureWrapMode = renderTexture.wrapMode;

        if (renderTexture is CustomRenderTexture customRenderTexture)
            customRenderTexture.Initialize();
    }
}
