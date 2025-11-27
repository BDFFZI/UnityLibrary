using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class KawaseBlur : MonoBehaviour, IProceduralTextureProcess
{
    [SerializeField] [ReadOnly] Shader shader;
    [SerializeField] [Range(1, 16)] int iterations = 5;
    [SerializeField] [Min(0)] float blurRadius = 2;

    static readonly int BlurRadiusID = Shader.PropertyToID("_BlurRadius");

    Material material;

    void OnEnable()
    {
        shader = Shader.Find("Hidden/BoxBlur");
        material = new Material(shader);
    }

    public void ProcessTexture(ScriptableRenderContext context, ref RenderingData renderingData,
        IProceduralTextureCanvas textureInfo, CommandBuffer cmd, RTHandle source, RTHandle destination)
    {
        for (int i = 0; i < iterations; i++)
        {
            cmd.SetGlobalFloat(BlurRadiusID, blurRadius * (i + 1));
            cmd.Blit(source, destination, material, 0);
            (source, destination) = (destination, source);
        }
        if (iterations % 2 == 0)
            cmd.Blit(source, destination);
    }
}
