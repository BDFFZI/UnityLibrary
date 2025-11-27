using System;
using System.Collections.Generic;
using UnityEngine;

public class TransformGraphDijkstra : MonoBehaviour, IRouteAlgorithm
{
    public void UpdatePaths(Transform origin)
    {
        if (graph.GetNode(origin) == null)
            throw new NullReferenceException("起点不在图中！");

        //重置路径缓冲区
        foreach (Transform key in paths.Keys)
        {
            //所有路径长度途径点置零
            Path path = paths[key];
            path.distance = float.MaxValue;
            path.waypoints.Clear();
            unknownWaypoints.Add(key); //将所有点设置为代检查点
        }

        //将起点设为第一个最近点，以启动遍历过程
        paths[origin].distance = 0;
        paths[origin].waypoints.Add(origin);

        while (unknownWaypoints.Count > 0)
        {
            //走下一个最短路径
            float nearestDistance = float.MaxValue;
            Transform nearestPoint = null;
            foreach (Transform key in unknownWaypoints)
            {
                Path path = paths[key];
                if (path.distance < nearestDistance)
                {
                    nearestDistance = path.distance;
                    nearestPoint = key;
                }
            }

            unknownWaypoints.Remove(nearestPoint);
            //以该节点更新新的可走路径信息
            Path prePath = paths[nearestPoint!];
            TransformGraph.Node nodeInfo = graph.GetNode(nearestPoint);
            int edgeCount = nodeInfo.Edges.Count;
            for (int i = 0; i < edgeCount; i++)
            {
                Transform edge = nodeInfo.Edges[i];
                float directDistance = nodeInfo.Distances[i];
                float lastTotalDistance = paths[edge].distance;
                if (prePath.distance + directDistance < lastTotalDistance)
                {
                    Path edgePath = paths[edge];
                    edgePath.distance = prePath.distance + directDistance;
                    edgePath.waypoints.Clear();
                    edgePath.waypoints.AddRange(prePath.waypoints);
                    edgePath.waypoints.Add(edge);
                }
            }
        }
    }
    public List<Transform> GetPath(Transform destination)
    {
        return paths[destination].waypoints;
    }

    public void Route(Transform origin, Transform destination, ref List<Transform> path)
    {
        UpdatePaths(origin);
        path = GetPath(destination);
    }


    [SerializeField] TransformGraph graph;

    class Path
    {
        public Path(float distance, List<Transform> waypoints)
        {
            this.distance = distance;
            this.waypoints = waypoints;
        }

        public float distance;
        public readonly List<Transform> waypoints;
    }


    Dictionary<Transform, Path> paths;
    HashSet<Transform> unknownWaypoints;

    void Awake()
    {
        int nodeCount = graph.Nodes.Count;
        paths = new Dictionary<Transform, Path>(nodeCount);
        foreach (TransformGraph.Node node in graph.Nodes)
            paths[node.Vertex] = new Path(float.MaxValue, new List<Transform>());
        unknownWaypoints = new HashSet<Transform>(nodeCount);
    }
}
