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
        return objs.Where(obj => layerMask == (layerMask | (1 << obj.layer)));
    }
}