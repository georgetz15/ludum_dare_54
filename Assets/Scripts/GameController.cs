using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
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
}