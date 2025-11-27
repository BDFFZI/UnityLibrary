using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public abstract class PostProcessPass : RenderPass
{
    public abstract Material Material { get; }

    RTHandle tempTarget;

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType != CameraType.Game)
            return;

        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        RenderingUtils.ReAllocateIfNeeded(ref tempTarget,
            new RenderTextureDescriptor(descriptor.width, descriptor.height, descriptor.colorFormat), FilterMode.Bilinear, TextureWrapMode.Clamp,
            name: "_TempTarget"
        );
        
        CommandBuffer.Blit(renderingData.cameraData.renderer.cameraColorTargetHandle, tempTarget);
        CommandBuffer.Blit(tempTarget, renderingData.cameraData.renderer.cameraColorTargetHandle, Material, 0);
        context.ExecuteCommandBuffer(CommandBuffer);
    }

    public override void OnDestroy()
    {
        RTHandles.Release(tempTarget);
    }
}
