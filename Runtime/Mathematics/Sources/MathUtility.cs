using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class MathUtility
{
    public static float ComponentSum(this float3 vector)
    {
        return vector.x + vector.y + vector.z;
    }

    public static float3 CatmullRom(float3 p0, float3 p1, float3 p2, float3 p3, float t)
    {
        float3 t2 = t * t;
        float3 t3 = t2 * t;
        return p0 * (-0.5f * t3 + t2 - 0.5f * t) +
               p1 * (1.5f * t3 - 2.5f * t2 + 1.0f) +
               p2 * (-1.5f * t3 + 2.0f * t2 + 0.5f * t) +
               p3 * (0.5f * t3 - 0.5f * t2);
    }

    public static void CatmullRomPath(List<Transform> waypoints, ref List<float3> controlPoints, ref List<float3> pathPoints, int precision)
    {
        controlPoints.Clear();
        controlPoints.Add(waypoints[0].position * 2 - waypoints[1].position);
        foreach (Transform waypoint in waypoints)
            controlPoints.Add(waypoint.position);
        controlPoints.Add(waypoints[^1].position * 2 - waypoints[^2].position);

        pathPoints.Clear();
        for (int i = 0; i < controlPoints.Count - 3; i++)
        {
            float3 p0 = controlPoints[i];
            float3 p1 = controlPoints[i + 1];
            float3 p2 = controlPoints[i + 2];
            float3 p3 = controlPoints[i + 3];

            for (int j = 0; j < precision; j++)
            {
                float t = (float)j / precision;
                float3 point = CatmullRom(p0, p1, p2, p3, t);
                pathPoints.Add(point);
            }
        }
        pathPoints.Add(controlPoints[^2]);
    }

    public static float NormalizeAngle(float angle)
    {
        return (angle % 360 + 360) % 360;
    }
    public static Vector3 NormalizeAngle(Vector3 angle)
    {
        angle.x = NormalizeAngle(angle.x);
        angle.y = NormalizeAngle(angle.y);
        angle.z = NormalizeAngle(angle.z);
        return angle;
    }
}
