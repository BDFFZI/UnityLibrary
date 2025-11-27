using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TransformGraphRouteTest : SerializedMonoBehaviour
{
    [SerializeField] TransformGraph graph;
    [SerializeField] IRouteAlgorithm routeAlgorithm;
    [SerializeField] Transform destination;

    List<Transform> path;

    void Awake()
    {
        path = new List<Transform>();
    }

    void Update()
    {
        TransformGraph.Node node = graph.GetNode(transform.position);
        routeAlgorithm.Route(node.Vertex, destination, ref path);
        for (int i = 1; i < path.Count; i++)
            Debug.DrawLine(path[i - 1].position, path[i].position, Color.red);
    }
}
