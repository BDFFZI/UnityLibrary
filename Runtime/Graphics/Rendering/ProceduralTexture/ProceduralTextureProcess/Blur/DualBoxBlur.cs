using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class DualBoxBlur : DualBlur
{
    protected override string GetBlurShaderName() => "Hidden/BoxBlur";
    protected override void SetBlurParameters(CommandBuffer cmd) { }
}
