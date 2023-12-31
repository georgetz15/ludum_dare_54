using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;
using UnityEngine.Events;

public class SpaceShipController : MonoBehaviour
{
    public static SpaceShipController Instance;
    [SerializeField] private float range = 5.0f;
    [SerializeField] private int maxCargoCapacity = 50;

    [SerializeField] private UnityEvent onCargoChanged;
    private int _cargo;

    [SerializeField] private float flyingSpeed = 3.0f;
    public bool IsTravelling { get; private set; }
    private Transform _destination;
    [SerializeField] private UnityEvent<Transform> onTravelFinished = new();
    [SerializeField] private GameObject gameController;
    private Queue<Transform> _path = new Queue<Transform>();
    [SerializeField] private UnityEvent<GameObject> onPathTravelFinished = new();

    [SerializeField] private UnityEvent<int> onCargoUpgraded = new();
    [SerializeField] private UnityEvent<float> onRangeUpgraded = new();
    private int _credits = 0;
    [SerializeField] private UnityEvent<int> onCreditsUpdate = new();

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

    public int Credits
    {
        get => _credits;
        set
        {
            _credits = value;
            onCreditsUpdate.Invoke(_credits);
        }
    }

    // private GameObject _currentPlane
    public GameObject CurrentPlanet { get; private set; }

    public void TravelWithPath(GameObject destination)
    {
        var gc = gameController.GetComponent<GameController>();
        if (gc is null) return;
        if (IsTravelling) return;

        var path = gc.mapController.GetShortestPath(CurrentPlanet.transform, destination.transform);
        if (path.Count > 0) path.RemoveAt(0);
        _path = new Queue<Transform>(path);
        TryTravelNext();
    }

    public void TravelTo(GameObject destination)
    {
        if (IsTravelling) return;
        if (destination == CurrentPlanet) return;

        // transform.position = destination.transform.position;
        _destination = destination.transform;
        transform.up = _destination.position - transform.position;
        IsTravelling = true;
    }

    private void TryTravelNext()
    {
        _path.TryDequeue(out var dest);
        if (dest is null)
        {
            onPathTravelFinished.Invoke(CurrentPlanet);
            return;
        }

        TravelTo(dest.gameObject);
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Cargo = 0;
        Credits = 0;
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

            onTravelFinished.Invoke(CurrentPlanet.transform);
            TryTravelNext();
        }
    }

    public void SetRandomPlanetStartingPoint()
    {
        var planets = GameController.GetObjectsInLayer(LayerMask.GetMask("Planets")).ToList();
        var i = Random.Range(0, planets.Count - 1);
        TravelTo(planets[i]);
        CurrentPlanet = planets[i];
    }

    public void UpgradeCargo(int addedSlots)
    {
        MaxCargoCapacity += addedSlots;
        onCargoUpgraded.Invoke(MaxCargoCapacity);
    }

    public void UpgradeRange(float addedRange)
    {
        Range += addedRange;
        onRangeUpgraded.Invoke(Range);
    }

    public void OnTaskCompletion(PlayerTask task)
    {
        Credits += task.Reward;
    }
}