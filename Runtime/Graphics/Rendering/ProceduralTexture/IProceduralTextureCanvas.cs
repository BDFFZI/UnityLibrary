using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public interface IProceduralTextureCanvas
{
    public RTHandle Texture { get; }

    public string TextureName { get; }
    public RenderTextureDescriptor TextureDescriptor { get; }
    public FilterMode TextureFilterMode { get; }
    public TextureWrapMode TextureWrapMode { get; }
    public bool RequestTexture(ScriptableRenderContext context, ref RenderingData renderingData);
}
