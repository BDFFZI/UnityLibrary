using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class Blend : SerializedMonoBehaviour, IProceduralTextureProcess
{
    enum BlendMode
    {
        Addition,
        Multiply,
        Tradition,
    }

    [SerializeField] [ReadOnly] Shader shader;
    [SerializeField] [BoxGroup("BlendTex")] [HideLabel] [EnableIf("@blendTexB == null")] IProceduralTextureCanvas blendTexA;
    [SerializeField] [BoxGroup("BlendTex")] [HideLabel] [EnableIf("@blendTexA == null ")] Texture blendTexB;
    [SerializeField] BlendMode blendMode;

    static readonly int BlendTex = Shader.PropertyToID("_BlendTex");

    Material material;

    void OnEnable()
    {
        shader = Shader.Find("Hidden/Blend");
        material = new Material(shader);
    }

    public void ProcessTexture(ScriptableRenderContext context, ref RenderingData renderingData,
        IProceduralTextureCanvas textureInfo, CommandBuffer cmd, RTHandle source, RTHandle destination)
    {
        cmd.SetGlobalTexture(BlendTex, blendTexA != null ? blendTexA.Texture : blendTexB);
        switch (blendMode)
        {
            case BlendMode.Addition: cmd.Blit(source, destination, material, 0); break;
            case BlendMode.Multiply: cmd.Blit(source, destination, material, 1); break;
            case BlendMode.Tradition: cmd.Blit(source, destination, material, 2); break;
        }
    }
}
