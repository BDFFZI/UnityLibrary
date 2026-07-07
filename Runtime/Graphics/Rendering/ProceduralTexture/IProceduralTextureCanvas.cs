using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public interface IProceduralTextureCanvas : IProceduralTextureSource
{
    public string TextureName { get; }
    public RenderTextureDescriptor TextureDescriptor { get; }
    public FilterMode TextureFilterMode { get; }
    public TextureWrapMode TextureWrapMode { get; }
    public bool RequestTexture(ScriptableRenderContext context, ref RenderingData renderingData);
}
