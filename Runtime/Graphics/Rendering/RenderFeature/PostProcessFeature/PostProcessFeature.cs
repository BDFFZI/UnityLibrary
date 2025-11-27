using UnityEngine;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class PostProcessFeature<TPass> : RenderFeature<TPass>
    where TPass : RenderPass, new()
{
    public override RenderPassEvent RenderQueue => RenderPassEvent.BeforeRenderingPostProcessing;
}
