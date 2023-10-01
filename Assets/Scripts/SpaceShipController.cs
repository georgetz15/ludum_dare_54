using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SpaceShipController : MonoBehaviour
{
    [SerializeField] private float range = 5.0f;
    [SerializeField] private int maxCargoCapacity = 6;

    [SerializeField] private UnityEvent onCargoChanged;
    private int _cargo;

    [SerializeField] private float flyingSpeed = 3.0f;
    public bool IsTravelling { get; private set; }
    private Transform _destination;
    [SerializeField] private UnityEvent<Transform> onTravelFinished = new();

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
        if (IsTravelling) return;
        if (destination == CurrentPlanet) return;
        
        // transform.position = destination.transform.position;
        _destination = destination.transform;
        transform.up = _destination.position - transform.position;
        IsTravelling = true;
    }
    
    // Start is called before the first frame update
    private void Awake()
    {
        Cargo = 0;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!IsTravelling) return;
        
        var step = flyingSpeed * Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _destination.position, step);

        // Arrived
        if (Vector3.Distance(transform.position, _destination.position) < 0.001f)
        {
            CurrentPlanet = _destination.gameObject;
            IsTravelling = false;
            _destination = null;

            onTravelFinished.Invoke(_destination);
        }
    }

    public void SetRandomPlanetStartingPoint()
    {
        var planets = GameController.GetObjectsInLayer(LayerMask.GetMask("Planets")).ToList();
        var i = Random.Range(0, planets.Count - 1);
        TravelTo(planets[i]);
    }
}