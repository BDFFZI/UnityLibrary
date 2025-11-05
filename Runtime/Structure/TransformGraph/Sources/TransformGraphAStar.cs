using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class TransformGraphAStar : MonoBehaviour, IRouteAlgorithm
{
    public TransformGraph Graph { get => graph; set => graph = value; }

    public void Route(Transform origin, Transform destination, ref List<Transform> path)
    {
        path.Clear();

        optionalPoints.Clear(); //可以行走的点位
        usedPoints.Clear(); //已被走过的节点
        nodeToWaypoint.Clear(); //图节点转路径点（其含成本参数）
        nodeToLastNode.Clear(); //记录每个节点的上一个节点

        //加入起点
        optionalPoints.Add(0, new Waypoint(origin));
        nodeToLastNode[origin] = null;

        while (optionalPoints.Count > 0)
        {
            //走到下一个最低成本点
            int nearestPointIndex = optionalPoints.Count - 1;
            Waypoint nearestPoint = optionalPoints.Values[nearestPointIndex];
            optionalPoints.RemoveAt(nearestPointIndex);
            usedPoints.Add(nearestPoint.node);

            //找到终点
            if (nearestPoint.node == destination)
            {
                Transform currentNode = nearestPoint.node;
                while (currentNode != null)
                {
                    path.Add(currentNode);
                    currentNode = nodeToLastNode[currentNode];
                }
                path.Reverse();
                break;
            }

            foreach (Transform edge in graph.GetNode(nearestPoint.node).Edges)
            {
                if (usedPoints.Contains(edge))
                    continue;

                Waypoint newPoint = new Waypoint(nearestPoint, edge, destination);
                if (nodeToWaypoint.TryGetValue(edge, out Waypoint oldPoint) == false)
                {
                    optionalPoints.Add(-newPoint.totalCost, newPoint);
                    nodeToWaypoint[edge] = newPoint;
                    nodeToLastNode[edge] = nearestPoint.node;
                }
                else if (newPoint.totalCost < oldPoint.totalCost)
                {
                    optionalPoints.Remove(-oldPoint.totalCost);
                    optionalPoints.Add(-newPoint.totalCost, newPoint);
                    nodeToWaypoint[edge] = newPoint;
                    nodeToLastNode[edge] = nearestPoint.node;
                }
            }
        }
    }

    [SerializeField] TransformGraph graph;

    struct Waypoint : IComparable<Waypoint>
    {
        public Waypoint(Waypoint lastWaypoint, Transform node, Transform destination)
        {
            realCost = lastWaypoint.realCost + Vector3.Distance(lastWaypoint.node.position, node.position);
            estimatedCost = math.abs(destination.position - node.position).ComponentSum();
            totalCost = realCost + estimatedCost;
            this.node = node;
        }
        public Waypoint(Transform node)
        {
            realCost = 0;
            estimatedCost = 0;
            totalCost = 0;
            this.node = node;
        }


        public int CompareTo(Waypoint other)
        {
            int compare = totalCost.CompareTo(other.totalCost);
            return compare;
        }

        public float realCost;
        public float estimatedCost;
        public float totalCost;
        public Transform node;
    }

    SortedList<float, Waypoint> optionalPoints;
    HashSet<Transform> usedPoints;
    Dictionary<Transform, Waypoint> nodeToWaypoint;
    Dictionary<Transform, Transform> nodeToLastNode;

    void Awake()
    {
        optionalPoints = new SortedList<float, Waypoint>();
        usedPoints = new HashSet<Transform>();
        nodeToWaypoint = new Dictionary<Transform, Waypoint>();
        nodeToLastNode = new Dictionary<Transform, Transform>();
    }
}
