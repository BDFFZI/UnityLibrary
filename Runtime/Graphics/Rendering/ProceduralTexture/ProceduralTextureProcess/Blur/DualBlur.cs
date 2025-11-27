using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public abstract class DualBlur : MonoBehaviour, IProceduralTextureProcess
{
    protected abstract string GetBlurShaderName();
    protected abstract void SetBlurParameters(CommandBuffer cmd);

    [SerializeField] [ReadOnly] Shader shader;
    [SerializeField] [Range(1, 8)] int blurIterations = 4;
    [SerializeField] [Min(0)] float blurRadius = 2;

    static readonly int BlurRadiusID = Shader.PropertyToID("_BlurRadius");

    Material material;
    List<RenderTexture> temporaryRTBuffer;

    void OnEnable()
    {
        shader = Shader.Find(GetBlurShaderName());
        material = new Material(shader);
        temporaryRTBuffer = new List<RenderTexture>();
    }

    public void ProcessTexture(ScriptableRenderContext context, ref RenderingData renderingData,
        IProceduralTextureCanvas textureInfo, CommandBuffer cmd, RTHandle source, RTHandle destination)
    {
        cmd.SetGlobalFloat(BlurRadiusID, blurRadius);
        SetBlurParameters(cmd);

        //创建临时纹理
        RenderTextureDescriptor descriptor = textureInfo.TextureDescriptor;
        for (int i = 0; i < blurIterations; i++)
        {
            descriptor.width /= 2;
            descriptor.height /= 2;
            temporaryRTBuffer.Add(RenderTexture.GetTemporary(descriptor)); //升降采样模糊
        }

        cmd.Blit(source, temporaryRTBuffer[0], material, 0);
        for (int i = 1; i < blurIterations; i++)
            cmd.Blit(temporaryRTBuffer[i - 1], temporaryRTBuffer[i], material, 0);
        for (int i = blurIterations - 1; i > 0; i--)
            cmd.Blit(temporaryRTBuffer[i], temporaryRTBuffer[i - 1], material, 0);
        cmd.Blit(temporaryRTBuffer[0], destination, material, 0);

        //回收临时纹理
        for (int i = temporaryRTBuffer.Count - 1; i >= 0; i--)
            RenderTexture.ReleaseTemporary(temporaryRTBuffer[i]);
        temporaryRTBuffer.Clear();
    }
}
