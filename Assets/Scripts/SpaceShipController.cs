using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SpaceShipController : MonoBehaviour
{
    [SerializeField] private float range = 5.0f;
    [SerializeField] private int maxCargoCapacity = 6;

    [SerializeField] private UnityEvent onCargoChanged;
    private int _cargo;

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

    public int Cargo
    {
        get => _cargo;
        set
        {
            _cargo = value;
            onCargoChanged.Invoke();
        }
    }

    // private GameObject _currentPlane
    public GameObject CurrentPlanet { get; private set; }

    public void TravelTo(GameObject destination)
    {
        transform.position = destination.transform.position;
        CurrentPlanet = destination;
    }


    // Start is called before the first frame update
    private void Awake()
    {
        Cargo = 0;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void setRandomPlanetStartingPoint()
    {
        var planets = GameController.GetObjectsInLayer(LayerMask.GetMask("Planets")).ToList();
        var i = Random.Range(0, planets.Count - 1);
        TravelTo(planets[i]);
    }
}