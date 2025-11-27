using UnityEngine;

public class CameraOutlineTexture : RenderTextureFeature<OutlineTexturePass>
{
    [SerializeField] float lineWidth = 1;
    [SerializeField] float depthEdgeThreshold = 0.25f;
    [SerializeField] bool enableNormalEdge = true;
    [SerializeField] float normalEdgeThreshold = 0.3f;
    [SerializeField] float edgeFaceThreshold = 0.58f;

    static readonly int LineWidthId = Shader.PropertyToID("_LineWidth");
    static readonly int DepthEdgeThresholdId = Shader.PropertyToID("_DepthEdgeThreshold");
    static readonly int NormalEdgeThresholdId = Shader.PropertyToID("_NormalEdgeThreshold");
    static readonly int EdgeFaceThresholdId = Shader.PropertyToID("_EdgeFaceThreshold");
    static readonly int NormalEdgeID = Shader.PropertyToID("_NormalEdge");

    protected override OutlineTexturePass CreatePass()
    {
        OutlineTexturePass pass = base.CreatePass();

        pass.RenderTextureProvider.RenderTarget.rt.filterMode = FilterMode.Bilinear;
        return pass;
    }

    protected override void SetupPass(OutlineTexturePass pass)
    {
        base.SetupPass(pass);

        Shader.SetGlobalFloat(LineWidthId, lineWidth);
        Shader.SetGlobalFloat(DepthEdgeThresholdId, depthEdgeThreshold);
        Shader.SetGlobalInt(NormalEdgeID, enableNormalEdge ? 1 : 0);
        Shader.SetGlobalFloat(NormalEdgeThresholdId, normalEdgeThreshold);
        Shader.SetGlobalFloat(EdgeFaceThresholdId, edgeFaceThreshold);
    }
}


public class OutlineTexturePass : RenderTexturePassByMaterial
{
    public override Material Material { get; } = new Material(Shader.Find("RenderFeature/CameraOutlineTexture"));
}
