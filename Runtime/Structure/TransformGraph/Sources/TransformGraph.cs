using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class TransformGraph : MonoBehaviour
{
    [Serializable]
    public class Node
    {
        public Node(Transform vertex)
        {
            this.vertex = vertex;
            edges = new List<Transform>();
        }
        public Node()
        {
            vertex = null;
            edges = new List<Transform>();
        }

        public Transform Vertex { get => vertex; set => vertex = value; }
        public List<Transform> Edges { get => edges; set => edges = value; }
        public float[] Distances => distances;

        public void UpdateDistance()
        {
            distances = new float[Edges.Count];
            for (int i = 0; i < edges.Count; i++)
                distances[i] = Vector3.Distance(edges[i].transform.position, Vertex.position);
        }

        [SerializeField] Transform vertex;
        [SerializeField] List<Transform> edges;

        float[] distances;
    }

    public List<Node> Nodes => nodes;
    public Node GetNode(Transform vertex)
    {
        return vertexToNode.GetValueOrDefault(vertex);
    }
    public Node GetNode(Vector3 position)
    {
        float nearestDistance = float.MaxValue;
        Node nearestPoint = null;
        foreach (Node node in nodes)
        {
            float distance = Vector3.Distance(position, node.Vertex.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestPoint = node;
            }
        }
        return nearestPoint;
    }

    [SerializeField] List<Node> nodes;

    Dictionary<Transform, Node> vertexToNode;

    void Awake()
    {
        vertexToNode = new Dictionary<Transform, Node>();
        foreach (Node node in nodes)
        {
            vertexToNode[node.Vertex] = node;
            node.UpdateDistance();
        }
    }
#if UNITY_EDITOR
    [Button]
    void SetNodesBaseSelection()
    {
        nodes.Clear();
        foreach (Transform transform in Selection.transforms)
            nodes.Add(new Node(transform));
    }
    [Button]
    void SetEdgesBaseNodeDistance(float maxDistance)
    {
        for (int i = 0; i < nodes.Count; i++)
            nodes[i].Edges.Clear();
        for (int i = 0; i < nodes.Count; i++)
        {
            Node origin = nodes[i];
            for (int j = i + 1; j < nodes.Count; j++)
            {
                Node destination = nodes[j];
                if (Vector3.Distance(origin.Vertex.position, destination.Vertex.position) <= maxDistance)
                {
                    origin.Edges.Add(destination.Vertex);
                    destination.Edges.Add(origin.Vertex);
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        foreach (Node node in nodes)
        {
            foreach (Transform edge in node.Edges)
            {
                Gizmos.DrawLine(node.Vertex.position, edge.transform.position);
            }
        }
    }
#endif
}
