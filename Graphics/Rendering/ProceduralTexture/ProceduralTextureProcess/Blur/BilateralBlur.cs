using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class BilateralBlur : MonoBehaviour, IProceduralTextureProcess
{
    [SerializeField] [ReadOnly] Shader shader;
    [SerializeField] [Range(1, 16)] int iterations = 3;
    [SerializeField] [Min(0)] float blurRadius = 1;
    [SerializeField] [Range(0.0001f, 0.2f)] float spatialDeviation = 0.1f;
    [SerializeField] [Range(0.0001f, 0.2f)] float tonalDeviation = 0.1f;

    static readonly int BlurRadiusID = Shader.PropertyToID("_BlurRadius");
    static readonly int SpatialDeviation = Shader.PropertyToID("_SpatialDeviation");
    static readonly int TonalDeviation = Shader.PropertyToID("_TonalDeviation");

    Material material;

    void OnEnable()
    {
        shader = Shader.Find("Hidden/BilateralBlur");
        material = new Material(shader);
    }

    public void ProcessTexture(ScriptableRenderContext context, ref RenderingData renderingData,
        IProceduralTextureCanvas textureInfo, CommandBuffer cmd, RTHandle source, RTHandle destination)
    {
        cmd.SetGlobalFloat(BlurRadiusID, blurRadius);
        cmd.SetGlobalFloat(SpatialDeviation, spatialDeviation);
        cmd.SetGlobalFloat(TonalDeviation, tonalDeviation);
        for (int i = 0; i < iterations; i++)
        {
            cmd.Blit(source, destination, material, 0);
            (source, destination) = (destination, source);
        }
        if (iterations % 2 == 0)
            cmd.Blit(source, destination);
    }
}
