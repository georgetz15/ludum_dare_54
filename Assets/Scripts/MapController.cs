using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private Dictionary<Transform, HashSet<Transform>> _pathGraph;
    [SerializeField] private GameObject edgePrefab;
    [SerializeField] private float lineWidth = 0.1f;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        // Create the path graph and show edges to the screen
        CleanupGraph();
        UpdatePathGraph();
        var edges = GetEdges(_pathGraph);
        foreach (var edge in edges)
        {
            Debug.Log($"Found edge {edge.Item1.name}, {edge.Item2.name}");
            CreateEdgeObject(edge.Item1, edge.Item2);
        }
    }

    private static IEnumerable<Tuple<Transform, Transform>> GetEdges(
        Dictionary<Transform, HashSet<Transform>> pathGraph)
    {
        return from nodeFrom in pathGraph.Keys
            from nodeTo in pathGraph[nodeFrom]
            select new Tuple<Transform, Transform>(nodeFrom, nodeTo);
    }

    // Planets with distance within range are connected
    private static Dictionary<Transform, HashSet<Transform>> GeneratePathGraph(float distance, LayerMask nodesLayer)
    {
        // Hardcode this to sth big
        // const float vertexSearchDistance = 1000f;
        // var hitColliders = new List<Collider>(
        //     Physics.OverlapSphere(Vector3.zero,
        //         vertexSearchDistance,
        //         nodesLayer
        //     ));
        var nodes = GameController.GetObjectsInLayer(nodesLayer);

        var graph = new Dictionary<Transform, HashSet<Transform>>();
        foreach (var node in nodes) graph.Add(node.transform, new HashSet<Transform>());

        var visited = new HashSet<Transform>();
        var newNodes = new Stack<Transform>(graph.Keys);

        while (newNodes.Count > 0)
        {
            var currentNode = newNodes.Pop();
            visited.Add(currentNode);

            // Find new nodes
            var hitColliders = new List<Collider>(
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
        var spaceshipController =
            GameObject.FindGameObjectWithTag("SpaceshipController").GetComponent<SpaceShipController>();
        var distance = spaceshipController.Range;
        _pathGraph = GeneratePathGraph(distance, LayerMask.GetMask("Planets"));
    }

    private void CreateEdgeObject(Transform a, Transform b)
    {
        // setup light path
        var newLightLine = Instantiate(edgePrefab, transform);
        var lr = newLightLine.GetComponent<LineRenderer>();
        var positions = new Vector3[2] { a.position, b.position };
        lr.SetPositions(positions);
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
    }

    private void CleanupGraph()
    {
        _pathGraph = new Dictionary<Transform, HashSet<Transform>>();
        var edges = GameController.GetObjectsInLayer(LayerMask.GetMask("Graph"));
        foreach (var edge in edges)
        {
            Destroy(edge);
        }
    }
}