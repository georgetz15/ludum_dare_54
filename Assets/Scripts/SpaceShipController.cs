using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    [SerializeField] public int MaxCargoCapacity { get; set; } = 6;
    
    [SerializeField] private float range = 5.0f;
    public float Range
    {
        get => range;
        set => range = value;
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}