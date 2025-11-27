using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class DepthToPosition : MonoBehaviour, IProceduralTextureProcess
{
    public void ProcessTexture(ScriptableRenderContext context, ref RenderingData renderingData,
        IProceduralTextureCanvas textureInfo, CommandBuffer cmd, RTHandle source, RTHandle destination)
    {
        cmd.SetGlobalMatrix(
            ClipToWorldID,
            Matrix4x4.Inverse(
                GL.GetGPUProjectionMatrix(renderingData.cameraData.camera.projectionMatrix, false) *
                renderingData.cameraData.camera.worldToCameraMatrix
            )
        );
        cmd.Blit(source, destination, material, 0);
    }

    [SerializeField] [ReadOnly] Shader shader;

    static readonly int ClipToWorldID = Shader.PropertyToID("_ClipToWorld");

    Material material;

    void OnEnable()
    {
        shader = Shader.Find("ProceduralTexture/DepthToPosition");
        material = new Material(shader);
    }
}
