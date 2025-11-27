using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class CameraPositionTexture : ScreenTextureCanvas
{
    public override bool RequestTexture(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (depthTexture == null)
            return false;

        return base.RequestTexture(context, ref renderingData);
    }

    protected override void InitializeDescriptor(ScriptableRenderContext context, ref RenderingData renderingData, ref RenderTextureDescriptor descriptor)
    {
        base.InitializeDescriptor(context, ref renderingData, ref descriptor);
        descriptor.graphicsFormat = GraphicsFormat.R32G32B32A32_SFloat;
    }

    protected override void InitializeTexture(ScriptableRenderContext context, ref RenderingData renderingData, CommandBuffer cmd)
    {
        cmd.SetGlobalMatrix(
            ClipToWorldID,
            Matrix4x4.Inverse(
                GL.GetGPUProjectionMatrix(renderingData.cameraData.camera.projectionMatrix, false) *
                renderingData.cameraData.camera.worldToCameraMatrix
            )
        );
        cmd.Blit(depthTexture.Texture, Texture, material, 0);
    }

    [SerializeField] [ReadOnly] Shader shader;
    [SerializeField] [Required] CameraDepthTexture depthTexture;

    static readonly int ClipToWorldID = Shader.PropertyToID("_ClipToWorld");

    Material material;

    void OnEnable()
    {
        shader = Shader.Find("ProceduralTexture/DepthToPosition");
        material = new Material(shader);
    }
}
