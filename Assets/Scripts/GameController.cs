using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public PlanetSpawner planetSpawner;
    public MapController mapController;

    [SerializeField] private UnityEvent<int> onDateChanged = new();

    private int _date = 0;

    // Start is called before the first frame update
    private void Start()

    {
        planetSpawner.GeneratePlanets();
        mapController.GenerateGraph();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public static IEnumerable<GameObject> GetObjectsInLayer(LayerMask layerMask)
    {
        var objs = new List<GameObject>(FindObjectsOfType<GameObject>());
        return objs.Where(obj => IsInLayerMask(obj.layer, layerMask));
    }

    public static bool IsInLayerMask(LayerMask layer, LayerMask mask)
    {
        return mask == (mask | (1 << layer));
    }

    public static bool IsInPlanetsLayer(LayerMask layer)
    {
        var layerMask = LayerMask.GetMask("Planets");
        return IsInLayerMask(layer, layerMask);
    }

    public static GameObject GetSpaceshipController()
    {
        return GameObject.FindGameObjectWithTag("SpaceshipController");
    }

    public void ProgressDate(int days = 1)
    {
        _date += days;
        onDateChanged.Invoke(_date);
    }
}