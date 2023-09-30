using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private Dictionary<Transform, HashSet<Transform>> _pathGraph;

    // Start is called before the first frame update
    private void Start()
    {
        UpdatePathGraph();
        var edges = GetEdges(_pathGraph);
        foreach (var edge in edges) Debug.Log($"Found edge {edge.Item1.name}, {edge.Item2.name}");
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private List<Tuple<Transform, Transform>> GetEdges(Dictionary<Transform, HashSet<Transform>> pathGraph)
    {
        var edges = new List<Tuple<Transform, Transform>>();
        foreach (var nodeFrom in pathGraph.Keys)
        foreach (var nodeTo in pathGraph[nodeFrom])
            edges.Add(new Tuple<Transform, Transform>(nodeFrom, nodeTo));

        return edges;
    }

    // Planets with distance within range are connected
    private static Dictionary<Transform, HashSet<Transform>> GeneratePathGraph(float distance, LayerMask nodesLayer)
    {
        // Hardcode this to sth big
        const float vertexSearchDistance = 1000f;
        var hitColliders = new List<Collider>(
            Physics.OverlapSphere(Vector3.zero,
                vertexSearchDistance,
                nodesLayer
            ));

        var graph = new Dictionary<Transform, HashSet<Transform>>();
        foreach (var collider in hitColliders) graph.Add(collider.transform, new HashSet<Transform>());

        var visited = new HashSet<Transform>();
        var newNodes = new Stack<Transform>(graph.Keys);

        while (newNodes.Count > 0)
        {
            var currentNode = newNodes.Pop();
            visited.Add(currentNode);

            // Find new nodes
            hitColliders = new List<Collider>(
                Physics.OverlapSphere(currentNode.position,
                    distance,
                    nodesLayer));
            foreach (var otherNode in
                     hitColliders
                         .Select(hitCollider => hitCollider.transform)
                         .Where(transform => !visited.Contains(transform)))
                graph[currentNode].Add(otherNode);
            // Keep graph "directional" to simplify things
            // when rendering edges
            // graph[otherNode].Add(currentNode);
        }

        return graph;
    }

    private void UpdatePathGraph()
    {
        var distance = 5;
        _pathGraph = GeneratePathGraph(distance, LayerMask.GetMask("Planets"));
    }
}