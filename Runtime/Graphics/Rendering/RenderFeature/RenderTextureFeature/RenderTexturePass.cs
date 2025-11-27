using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public abstract class RenderTexturePass : RenderPass
{
    protected override string PassName => RenderTextureProvider.RenderTarget.name;
    protected abstract void RenderTexture(ScriptableRenderContext context, ref RenderingData renderingData);

    public IRenderTextureProvider RenderTextureProvider { get; internal set; }
    public bool Preview { get; internal set; }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        base.OnCameraSetup(cmd, ref renderingData);

        RenderTextureProvider.OnCameraSetup(ref renderingData);

        ConfigureTarget(RenderTextureProvider.RenderTarget);
        ConfigureClear(ClearFlag.All, Color.clear);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        RenderTexture(context, ref renderingData);
        if (Preview)
            CommandBuffer.Blit(RenderTextureProvider.RenderTarget, renderingData.cameraData.renderer.cameraColorTargetHandle);
        CommandBuffer.SetGlobalTexture(RenderTextureProvider.RenderTarget.name, RenderTextureProvider.RenderTarget);

        context.ExecuteCommandBuffer(CommandBuffer);
    }
}

public abstract class RenderTexturePassByMaterial : RenderTexturePass
{
    public abstract Material Material { get; }
    protected override void RenderTexture(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer.Blit(null, RenderTextureProvider.RenderTarget, Material, 0);
    }
}
