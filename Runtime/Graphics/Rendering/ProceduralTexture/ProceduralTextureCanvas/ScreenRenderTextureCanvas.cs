using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenRenderTextureCanvas : ScreenTextureCanvas
{
    RTHandle depthTexture;

    public override bool RequestTexture(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        descriptor.width = Mathf.CeilToInt(descriptor.width * SizeScaling);
        descriptor.height = Mathf.CeilToInt(descriptor.height * SizeScaling);
        RenderingUtils.ReAllocateIfNeeded(ref depthTexture, descriptor, TextureFilterMode, TextureWrapMode);

        return base.RequestTexture(context, ref renderingData);
    }

    protected override void InitializeTexture(ScriptableRenderContext context, ref RenderingData renderingData, CommandBuffer cmd)
    {
        cmd.SetRenderTarget(Texture, depthTexture);
        cmd.ClearRenderTarget(clearFlags, clearColor, 1, 0);
        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (depthTexture != null)
            RTHandles.Release(depthTexture);
    }

    [SerializeField] RTClearFlags clearFlags = RTClearFlags.All;
    [SerializeField] Color clearColor = Color.clear;
}
