using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CatmullRomTest : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] int precision = 5;

    List<Transform> waypoints;
    List<float3> controlPoints;
    List<float3> pathPoints;

    void Start()
    {
        waypoints = new List<Transform>();
        foreach (Transform child in transform)
            waypoints.Add(child);
        controlPoints = new List<float3>();
        pathPoints = new List<float3>();
    }

    void Update()
    {
        MathUtility.CatmullRomPath(waypoints, ref controlPoints, ref pathPoints, precision);
        lineRenderer.positionCount = pathPoints.Count;
        for (int i = 0; i < pathPoints.Count; i++)
            lineRenderer.SetPosition(i, pathPoints[i]);
    }
}
