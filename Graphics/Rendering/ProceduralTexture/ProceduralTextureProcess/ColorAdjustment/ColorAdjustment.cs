using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class ColorAdjustment : MonoBehaviour, IProceduralTextureProcess
{
    [SerializeField] [ReadOnly] Shader shader;
    [SerializeField] [Range(0, 2)] float intensity = 1.0f;
    [SerializeField] [Range(0, 2)] float saturate = 1.0f;
    [SerializeField] [Range(0, 2)] float contrast = 1.0f;

    static readonly int SaturateID = Shader.PropertyToID("_Saturate");
    static readonly int IntensityID = Shader.PropertyToID("_Intensity");
    static readonly int ContrastID = Shader.PropertyToID("_Contrast");

    Material material;

    void OnEnable()
    {
        if (shader == null)
            shader = Shader.Find("ProceduralTexture/ColorAdjustment");
        material = new Material(shader);
    }

    public void ProcessTexture(ScriptableRenderContext context, ref RenderingData renderingData,
        IProceduralTextureCanvas textureInfo, CommandBuffer cmd, RTHandle source, RTHandle destination)
    {
        cmd.SetGlobalFloat(IntensityID, intensity);
        cmd.SetGlobalFloat(SaturateID, saturate);
        cmd.SetGlobalFloat(ContrastID, contrast);
        cmd.Blit(source, destination, material, 0);
    }
}
