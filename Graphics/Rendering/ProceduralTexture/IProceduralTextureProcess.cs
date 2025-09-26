using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public interface IProceduralTextureProcess
{
    // ReSharper disable once InconsistentNaming
    bool enabled => true;

    void ProcessTexture(ScriptableRenderContext context, ref RenderingData renderingData,
        IProceduralTextureCanvas textureInfo, CommandBuffer cmd, RTHandle source, RTHandle destination);
}
