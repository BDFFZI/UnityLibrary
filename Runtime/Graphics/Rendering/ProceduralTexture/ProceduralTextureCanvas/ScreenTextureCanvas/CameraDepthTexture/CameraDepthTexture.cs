using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class CameraDepthTexture : ScreenTextureCanvas
{
    protected override void InitializeDescriptor(ScriptableRenderContext context, ref RenderingData renderingData, ref RenderTextureDescriptor descriptor)
    {
        base.InitializeDescriptor(context, ref renderingData, ref descriptor);
        descriptor.graphicsFormat = GraphicsFormat.R32_SFloat;
    }

    protected override void InitializeTexture(ScriptableRenderContext context, ref RenderingData renderingData, CommandBuffer cmd)
    {
        cmd.Blit(renderingData.cameraData.renderer.cameraDepthTargetHandle, Texture, material);
    }

    [SerializeField] [ReadOnly] Shader shader;

    Material material;

    void OnEnable()
    {
        shader = Shader.Find("ProceduralTexture/CopyDepth");
        material = new Material(shader);
    }
}
