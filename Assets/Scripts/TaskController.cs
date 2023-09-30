using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;
using UnityEngine.Events;

public class TaskController : MonoBehaviour
{
    [field: SerializeReference] public SpaceShipController spaceShipController { get; set; }

    [SerializeField] public UnityEvent<PLayerTasks> onTaskCreate;

    private readonly string[] _cargoType =
    {
        "Scientific Instruments",
        "Rovers and Landers",
        "Habitat Modules",
        "Cargo for Space Stations",
        "Communication and Navigation",
        "Fuel and Propellant",
        "Construction Materials",
        "Greenhouses and Agricultural Equipment",
        "Tools and Equipment",
        "Power Generation and Storage",
        "Payloads for Commercial Ventures"
    };

    private readonly int _maxNumberOfAvailableTasks = 12;

    private List<GameObject> _planets;

    [SerializeField] public List<PLayerTasks> availableTasks = new();

    // Start is called before the first frame update
    private void Awake()
    {
        if (onTaskCreate == null)
            onTaskCreate = new UnityEvent<PLayerTasks>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (HowManyNewTasksWeNeedToGenerate() > 0) GenerateTasks();
    }

    private void GenerateTasks()
    {
        var maxCargoCapacity = spaceShipController.MaxCargoCapacity;
        var randomCargoTypeIndex = Random.Range(0, _cargoType.Length);
        _planets = MapController.GetPlanets().ToList();
        var planetFrom = _planets[Random.Range(0, _planets.Count - 1)];
        var planetTo = _planets.Where(x => x != planetFrom).ToList()[Random.Range(0, _planets.Count - 2)];
        var newTask = new PLayerTasks
        {
            CargoName = _cargoType[randomCargoTypeIndex],
            CargoUnits = Random.Range(1, maxCargoCapacity),
            PlanetFrom = planetFrom,
            PlanetTo = planetTo,
            StartDateIssued = 0,
            DeliveryTick = 10
        };
        availableTasks.Add(newTask);
        Debug.Log($"new task name {_cargoType[randomCargoTypeIndex]}");

        onTaskCreate.Invoke(newTask);
    }

    private int HowManyNewTasksWeNeedToGenerate()
    {
        return _maxNumberOfAvailableTasks - availableTasks.Count;
    }
}