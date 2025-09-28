using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraTextureCanvas : MonoBehaviour, IProceduralTextureCanvas
{
    public RTHandle Texture { get; private set; }
    public string TextureName => Texture.name;
    public RenderTextureDescriptor TextureDescriptor { get; private set; }
    public FilterMode TextureFilterMode => FilterMode.Bilinear;
    public TextureWrapMode TextureWrapMode => TextureWrapMode.Clamp;
    public bool RequestTexture(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        Texture = renderingData.cameraData.renderer.cameraColorTargetHandle;
        TextureDescriptor = renderingData.cameraData.cameraTargetDescriptor;

        return true;
    }
}
