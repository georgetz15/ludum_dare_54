using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.Events;

public class TaskController : MonoBehaviour
{
    
    private readonly int _maxNumberOfAvailableTasks  = 12;
    
    [field: SerializeReference]
    public SpaceShipController spaceShipController { get; set; }
    
    [SerializeField]
    public List<PLayerTasks> availableTasks = new() { };

    [SerializeField] public UnityEvent<PLayerTasks> onTaskCreate;
        
    private readonly string[] _cargoType = {
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
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (HowManyNewTasksWeNeedToGenerate() > 0)
        {
            GenerateTasks();
        }
    }

    private void GenerateTasks()
    {
        var maxCargoCapacity = spaceShipController.MaxCargoCapacity;
        var randomCargoTypeIndex = Random.Range(0, _cargoType.Length);
        var newTask = new PLayerTasks()
        {
            CargoName = _cargoType[randomCargoTypeIndex],
            CargoUnits = Random.Range(1, maxCargoCapacity),
            PlanetFrom = null,
            PlanetTo = null,
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
