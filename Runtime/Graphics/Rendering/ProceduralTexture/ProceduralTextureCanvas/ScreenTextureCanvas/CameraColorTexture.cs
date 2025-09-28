using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraColorTexture : ScreenTextureCanvas
{
    protected override void InitializeTexture(ScriptableRenderContext context, ref RenderingData renderingData, CommandBuffer cmd)
    {
        cmd.Blit(renderingData.cameraData.renderer.cameraColorTargetHandle, Texture);
    }
}
