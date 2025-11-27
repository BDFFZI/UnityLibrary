using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class CameraNormalTexture : ScreenRenderTextureCanvas
{
    [SerializeField] [ReadOnly] Shader shader;

    Material material;
    List<ShaderTagId> shaderTags;

    void OnEnable()
    {
        shader = Shader.Find("ProceduralTexture/CameraNormalTexture");
        material = new Material(shader);
        shaderTags = new List<ShaderTagId>() {
            new ShaderTagId("DepthOnly"),
        };
    }

    protected override void InitializeTexture(ScriptableRenderContext context, ref RenderingData renderingData, CommandBuffer cmd)
    {
        base.InitializeTexture(context, ref renderingData, cmd);

        //渲染
        {
            DrawingSettings drawingSettings = RenderingUtils.CreateDrawingSettings(shaderTags, ref renderingData, SortingCriteria.CommonOpaque);
            drawingSettings.overrideMaterial = material;

            FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.all);

            context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);
        }
    }
}
