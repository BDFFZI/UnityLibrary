using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class DrawLayers : ScriptableRendererFeature
{
    [SerializeField] LayerMask[] targetLayers;
    [SerializeField] RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;

    DrawLayersPass drawLayersPass;

    public override void Create()
    {
        drawLayersPass = new DrawLayersPass();
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        drawLayersPass.TargetLayers = targetLayers;
        drawLayersPass.renderPassEvent = renderPassEvent;
        renderer.EnqueuePass(drawLayersPass);
    }
}

public class DrawLayersPass : ScriptableRenderPass
{
    public LayerMask[] TargetLayers { get; set; }

    readonly List<ShaderTagId> shaderTagIds = new List<ShaderTagId>() {
        new ShaderTagId("UniversalForward"),
        new ShaderTagId("SRPDefaultUnlit"),
    };

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CullingResults cullingResults = renderingData.cullResults;
        DrawingSettings drawingSettings = CreateDrawingSettings(shaderTagIds, ref renderingData, SortingCriteria.CommonTransparent);
        FilteringSettings filteringSettings = FilteringSettings.defaultValue;

        foreach (LayerMask layerMask in TargetLayers)
        {
            filteringSettings.layerMask = layerMask;

            CommandBuffer commandBuffer = CommandBufferPool.Get("DrawLayersPass");
            commandBuffer.ClearRenderTarget(RTClearFlags.DepthStencil, Color.clear, 1, 0);
            context.ExecuteCommandBuffer(commandBuffer);
            context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
            CommandBufferPool.Release(commandBuffer);
        }
    }
}
