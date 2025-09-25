using Unity.Mathematics;

public static class MathematicsUtility
{
    public static float ComponentSum(this float3 vector)
    {
        return vector.x + vector.y + vector.z;
    }
}
