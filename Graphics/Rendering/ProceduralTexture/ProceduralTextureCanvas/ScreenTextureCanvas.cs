using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenTextureCanvas : MonoBehaviour, IProceduralTextureCanvas
{
    public RTHandle Texture => texture;
    public string TextureName => Texture.name;
    public RenderTextureDescriptor TextureDescriptor => descriptor;
    public FilterMode TextureFilterMode { get => filterMode; set => filterMode = value; }
    public TextureWrapMode TextureWrapMode { get => wrapMode; set => wrapMode = value; }

    public float SizeScaling { get => sizeScaling; set => sizeScaling = value; }

    public virtual bool RequestTexture(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        //创建纹理
        InitializeDescriptor(context, ref renderingData, ref descriptor);
        RenderingUtils.ReAllocateIfNeeded(ref texture, TextureDescriptor, filterMode, wrapMode, name: textureName);

        //初始化纹理内容
        CommandBuffer cmd = CommandBufferPool.Get(textureName);
        InitializeTexture(context, ref renderingData, cmd);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);

        for (int i = 0; i < outputMaterials.Length; i++)
            outputMaterials[i].SetTexture(textureName, texture);

        return true;
    }

    protected virtual void InitializeDescriptor(ScriptableRenderContext context, ref RenderingData renderingData, ref RenderTextureDescriptor descriptor)
    {
        descriptor = renderingData.cameraData.cameraTargetDescriptor;
        descriptor.width = Mathf.CeilToInt(descriptor.width * sizeScaling);
        descriptor.height = Mathf.CeilToInt(descriptor.height * sizeScaling);
        descriptor.depthBufferBits = 0;
        if (format != GraphicsFormat.None)
            descriptor.graphicsFormat = format;
    }

    protected virtual void InitializeTexture(ScriptableRenderContext context, ref RenderingData renderingData, CommandBuffer cmd)
    {
        //默认不处理任何纹理内容
    }

    protected virtual void OnDestroy()
    {
        if (texture != null)
            RTHandles.Release(texture);
    }

    [SerializeField] FilterMode filterMode = FilterMode.Bilinear;
    [SerializeField] TextureWrapMode wrapMode = TextureWrapMode.Clamp;
    [SerializeField] GraphicsFormat format = GraphicsFormat.None;
    [SerializeField] [Range(0.001f, 2)] float sizeScaling = 1;
    [SerializeField] string textureName = "_MainTex";
    [SerializeField] Material[] outputMaterials;

    RTHandle texture;
    RenderTextureDescriptor descriptor;
}
