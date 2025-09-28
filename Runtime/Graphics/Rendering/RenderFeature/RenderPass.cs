using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public abstract class RenderPass : ScriptableRenderPass
{
    protected virtual string PassName => GetType().Name;
    protected CommandBuffer CommandBuffer => commandBuffer;

    CommandBuffer commandBuffer;

    public virtual void OnAwake()
    {
        commandBuffer = CommandBufferPool.Get(PassName);
    }
    public virtual void OnDestroy()
    {
        CommandBufferPool.Release(commandBuffer);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        base.OnCameraCleanup(cmd);
        commandBuffer.Clear();
    }
}
