using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[ExecuteAlways]
public class Bloom : MonoBehaviour, IProceduralTextureProcess
{
    [SerializeField] [ReadOnly] Shader bloomShader;
    [SerializeField] [ReadOnly] Shader blurShader;
    [SerializeField] [Min(0)] float threshold = 0.9f;
    [SerializeField] [Min(0)] float intensity = 1;
    [SerializeField] [Range(0, 1)] float scatter = 0.7f;
    [SerializeField] [Range(1, 4)] int downsample = 2;
    [SerializeField] [Range(1, 8)] int blurIterations = 4;
    [SerializeField] [Min(0)] float blurRadius = 2;

    static readonly int IntensityID = Shader.PropertyToID("_Intensity");
    static readonly int ThresholdID = Shader.PropertyToID("_Threshold");
    static readonly int BlurRadiusID = Shader.PropertyToID("_BlurRadius");
    static readonly int BloomTexID = Shader.PropertyToID("_BloomTex");
    static readonly int ScatterID = Shader.PropertyToID("_Scatter");

    Material bloomMaterial;
    Material blurMaterial;
    List<RenderTexture> temporaryRTBuffer;

    void OnEnable()
    {
        bloomShader = Shader.Find("ProceduralTexture/Bloom");
        blurShader = Shader.Find("ProceduralTexture/GaussianBlur");
        bloomMaterial = new Material(bloomShader);
        blurMaterial = new Material(blurShader);
        temporaryRTBuffer = new List<RenderTexture>();
    }

    public void ProcessTexture(ScriptableRenderContext context, ref RenderingData renderingData,
        IProceduralTextureCanvas textureInfo, CommandBuffer cmd, RTHandle source, RTHandle destination)
    {
        //创建临时纹理
        RenderTextureDescriptor descriptor = textureInfo.TextureDescriptor;
        descriptor.width /= downsample;
        descriptor.height /= downsample;
        temporaryRTBuffer.Add(RenderTexture.GetTemporary(descriptor)); //发光区域
        for (int i = -1; i < blurIterations; i++)
        {
            descriptor.width /= 2;
            descriptor.height /= 2;
            temporaryRTBuffer.Add(RenderTexture.GetTemporary(descriptor)); //横向模糊
            temporaryRTBuffer.Add(RenderTexture.GetTemporary(descriptor)); //纵向模糊
        }

        //进行实际渲染
        {
            cmd.SetGlobalFloat(IntensityID, intensity);
            cmd.SetGlobalFloat(ThresholdID, threshold);
            cmd.SetGlobalFloat(ScatterID, scatter);

            //找出发光区域
            cmd.Blit(source, temporaryRTBuffer[0], bloomMaterial, 0);

            //生成多级别模糊贴图
            for (int i = 0; i <= blurIterations; i++)
            {
                cmd.SetGlobalFloat(BlurRadiusID, blurRadius * 2);
                cmd.Blit(temporaryRTBuffer[i * 2], temporaryRTBuffer[i * 2 + 1], blurMaterial, 0);
                cmd.SetGlobalFloat(BlurRadiusID, blurRadius * 1);
                cmd.Blit(temporaryRTBuffer[i * 2 + 1], temporaryRTBuffer[i * 2 + 2], blurMaterial, 1);
            }

            //生成辉光效果
            RenderTexture bloomTexture = temporaryRTBuffer.Last(); //当前最底层模糊
            for (int i = blurIterations; i > 0; i--)
            {
                RenderTexture unBloomTexture = temporaryRTBuffer[i * 2]; //上一层模糊（更清晰和接近原图）
                RenderTexture outputTexture = temporaryRTBuffer[i * 2 - 1];

                cmd.SetGlobalTexture(BloomTexID, bloomTexture);
                cmd.Blit(unBloomTexture, outputTexture, bloomMaterial, 1); //通过散射插值清晰和模糊，来产生辉光效果
                bloomTexture = outputTexture;
            }

            //将最终的辉光效果叠加到原图
            cmd.SetGlobalTexture(BloomTexID, bloomTexture);
            cmd.Blit(source, destination, bloomMaterial, 2);
        }

        //回收临时纹理
        for (int i = temporaryRTBuffer.Count - 1; i >= 0; i--)
            RenderTexture.ReleaseTemporary(temporaryRTBuffer[i]);
        temporaryRTBuffer.Clear();
    }
}
