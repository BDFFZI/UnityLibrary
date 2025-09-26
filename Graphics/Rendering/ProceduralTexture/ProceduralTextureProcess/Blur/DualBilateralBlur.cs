using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class DualBilateralBlur : DualBlur
{
    [SerializeField] [Range(0.0001f, 0.1f)] float spatialDeviation = 1;
    [SerializeField] [Range(0.0001f, 0.1f)] float tonalDeviation = 1;

    static readonly int SpatialDeviation = Shader.PropertyToID("_SpatialDeviation");
    static readonly int TonalDeviation = Shader.PropertyToID("_TonalDeviation");

    protected override string GetBlurShaderName() => "Hidden/BilateralBlur";
    protected override void SetBlurParameters(CommandBuffer cmd)
    {
        cmd.SetGlobalFloat(SpatialDeviation, spatialDeviation);
        cmd.SetGlobalFloat(TonalDeviation, tonalDeviation);
    }
}
