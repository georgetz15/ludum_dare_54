#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private GameObject edgePrefab;
    [SerializeField] private float lineWidth = 0.1f;
    [SerializeField] private GameObject spaceshipConroller;
    [SerializeField] private Material edgeMaterial;
    [SerializeField] private Material activeEdgeMaterial;
    private List<GameObject> _cachedEdgesUIObjects = new();
    private List<Transform>? _cachedPath;

    private Dictionary<Transform, HashSet<Transform>>? _pathGraph;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void GenerateGraph()
    {
        // Create the path graph and show edges to the screen
        CleanupGraph();
        UpdatePathGraph();
        var edges = GetEdges(_pathGraph);
        foreach (var edge in edges)
        {
            Debug.Log($"Found edge {edge.Item1.name}, {edge.Item2.name}");
            var obj = CreateEdgeObject(edge.Item1, edge.Item2);
            _cachedEdgesUIObjects.Add(obj);
        }
    }

    private static IEnumerable<Tuple<Transform, Transform>> GetEdges(
        Dictionary<Transform, HashSet<Transform>> pathGraph)
    {
        var edges = new HashSet<Tuple<Transform, Transform>>();
        foreach (var nodeFrom in pathGraph.Keys)
        foreach (var nodeTo in pathGraph[nodeFrom])
            if (!edges.Contains(new Tuple<Transform, Transform>(nodeFrom, nodeTo)) &&
                !edges.Contains(new Tuple<Transform, Transform>(nodeTo, nodeFrom)))
                edges.Add(new Tuple<Transform, Transform>(nodeFrom, nodeTo));

        return edges;
    }

    // Planets with distance within range are connected
    private static Dictionary<Transform, HashSet<Transform>> GeneratePathGraph(float distance, LayerMask nodesLayer)
    {
        var nodes = GameController.GetObjectsInLayer(nodesLayer);

        var graph = nodes.ToDictionary(node => node.transform, node => new HashSet<Transform>());

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
            {
                graph[currentNode].Add(otherNode);
                graph[otherNode].Add(currentNode);
            }
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

    private GameObject CreateEdgeObject(Transform a, Transform b)
    {
        // setup light path
        var newLightLine = Instantiate(edgePrefab, transform);
        var lr = newLightLine.GetComponent<LineRenderer>();
        var positions = new Vector3[2] { a.position, b.position };
        lr.SetPositions(positions);
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;

        return newLightLine;
    }

    private void CleanupGraph()
    {
        _pathGraph = new Dictionary<Transform, HashSet<Transform>>();
        var edges = GameController.GetObjectsInLayer(LayerMask.GetMask("Graph"));
        foreach (var edge in edges) Destroy(edge);
        _cachedEdgesUIObjects = new List<GameObject>();
    }

    public static IEnumerable<GameObject> GetPlanets()
    {
        return GameController.GetObjectsInLayer(LayerMask.GetMask("Planets"));
    }

    public List<Transform> GetShortestPath(
        Transform from, Transform to)
    {
        if (_cachedPath is not null)
            if (_cachedPath.Count > 0)
                if (_cachedPath[0] == from && _cachedPath[^1] == to)
                    return _cachedPath;

        if (Bfs(_pathGraph, from, to, out var pred, out var dist) == false) return new List<Transform>();

        // List to store path
        var path = new List<Transform>();
        var crawl = to;
        path.Add(crawl);

        while (pred[crawl] != null)
        {
            path.Add(pred[crawl]);
            crawl = pred[crawl];
        }

        path.Reverse();
        return path;
    }

// a modified version of BFS that
// stores predecessor of each vertex
// in array pred and its distance
// from source in array dist
    private static bool Bfs(Dictionary<Transform, HashSet<Transform>> adj,
        Transform src, Transform dest, out Dictionary<Transform, Transform?> pred,
        out Dictionary<Transform, int> dist)
    {
        // a queue to maintain queue of
        // vertices whose adjacency list
        // is to be scanned as per normal
        // BFS algorithm using List of int type
        var queue = new Queue<Transform>();

        // bool array visited[] which
        // stores the information whether
        // ith vertex is reached at least
        // once in the Breadth first search
        // var visited = new bool[v];
        var visited = new HashSet<Transform>();

        // initially all vertices are
        // unvisited so v[i] for all i
        // is false and as no path is
        // yet constructed dist[i] for
        // all i set to infinity
        var verts = adj.Keys;
        dist = verts.ToDictionary(vert => vert, vert => int.MaxValue);
        // pred = verts.ToDictionary<Transform, Transform>(vert => vert, vert => null);
        pred = verts.ToDictionary<Transform?, Transform, Transform?>(vert => vert, vert => null);

        // now source is first to be
        // visited and distance from
        // source to itself should be 0
        visited.Add(src);
        dist[src] = 0;
        queue.Enqueue(src);

        // bfs Algorithm
        while (queue.Count > 0)
        {
            var u = queue.Dequeue();

            foreach (var v in adj[u].Where(v => !visited.Contains(v)))
            {
                visited.Add(v);
                dist[v] = dist[u] + 1;
                pred[v] = u;
                queue.Enqueue(v);
                if (v == dest) return true;
            }
        }

        return false;
    }

    public void OnPlanetHoverEnter(Transform planet)
    {
        var sc = spaceshipConroller.GetComponent<SpaceShipController>();
        if (sc is null) return;

        _cachedPath =
            new List<Transform>(
                GetShortestPath(sc.CurrentPlanet.transform,
                    planet));

        if (sc.IsTravelling) return;
        SetActiveEdgesMaterial(_cachedPath);
    }

    private void SetActiveEdgesMaterial(List<Transform> path)
    {
        for (var i = 0; i < path.Count - 1; i++)
        {
            var start = path[i];
            var end = path[i + 1];
            var edge = _cachedEdgesUIObjects.Find(x =>
            {
                var lr = x.GetComponent<LineRenderer>();
                var pos0 = lr.GetPosition(0);
                var pos1 = lr.GetPosition(1);

                var isFound = (pos0 == start.position && pos1 == end.position) ||
                              (pos1 == start.position && pos0 == end.position);

                return isFound;
            });

            // var mat = edge.GetComponent<Renderer>().material;
            // mat.SetColor("_BaseColor", Color.cyan);
            edge.GetComponent<Renderer>().material = activeEdgeMaterial;
        }
    }

    public void OnPlanetHoverExit(Transform planet)
    {
        var sc = spaceshipConroller.GetComponent<SpaceShipController>();
        if (sc is null) return;
        if (sc.IsTravelling) return;
        _cachedPath = null;

        SetEdgesDefaultMaterial();
    }

    private void SetEdgesDefaultMaterial()
    {
        foreach (var edge in _cachedEdgesUIObjects)
        {
            var mat = edge.GetComponent<Renderer>().material;
            // mat.SetColor("_BaseColor", _originalColor);
            edge.GetComponent<Renderer>().material = edgeMaterial;
        }
    }

    public void OnSpaceshipPathTravelFinished()
    {
        SetEdgesDefaultMaterial();
        if (_cachedPath is null) return;
        
        // Case where user hovered to different planet while travelling
        var sc = spaceshipConroller.GetComponent<SpaceShipController>();
        if (sc is null) return;
        _cachedPath =
            new List<Transform>(
                GetShortestPath(sc.CurrentPlanet.transform,
                    _cachedPath[^1]));
        SetActiveEdgesMaterial(_cachedPath);
    }
}