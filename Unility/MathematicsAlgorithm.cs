using Unity.Mathematics;
using UnityEngine;

public static class MathematicsAlgorithm
{
    public static float ComponentSum(this float3 vector)
    {
        return vector.x + vector.y + vector.z;
    }
}
