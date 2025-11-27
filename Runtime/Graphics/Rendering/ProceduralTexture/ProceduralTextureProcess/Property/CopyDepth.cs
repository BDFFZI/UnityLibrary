using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class CopyDepth : MonoBehaviour, IProceduralTextureProcess
{
    public void ProcessTexture(ScriptableRenderContext context, ref RenderingData renderingData,
        IProceduralTextureCanvas textureInfo, CommandBuffer cmd, RTHandle source, RTHandle destination)
    {
        cmd.Blit(renderingData.cameraData.renderer.cameraDepthTargetHandle, destination, material);
    }

    [SerializeField] [ReadOnly] Shader shader;

    Material material;

    void OnEnable()
    {
        shader = Shader.Find("ProceduralTexture/CopyDepth");
        material = new Material(shader);
    }
}
