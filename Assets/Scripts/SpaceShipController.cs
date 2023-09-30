using UnityEngine;
using UnityEngine.Serialization;

public class SpaceShipController : MonoBehaviour
{
    public int MaxCargoCapacity
    {
        get => maxCargoCapacity;
        set => maxCargoCapacity = value;
    }

    public float Range
    {
        get => range;
        set => range = value;
    }

    [SerializeField] private float range = 5.0f;
    [SerializeField] private int maxCargoCapacity = 6;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}