using System.Collections.Generic;
using System.Linq;
using Models;
using Tasks;
using UnityEngine;
using UnityEngine.Events;

public class TaskController : MonoBehaviour
{
    [field: SerializeReference] public SpaceShipController spaceShipController { get; set; }

    [SerializeField] public UnityEvent<PlayerTasks> onTaskCreate;

    
    private readonly int _maxNumberOfAvailableTasks = 12;

    private List<GameObject> _planets;

    [SerializeField] public List<PlayerTasks> availableTasks = new();

    // Start is called before the first frame update
    private void Awake()
    {
        if (onTaskCreate == null)
            onTaskCreate = new UnityEvent<PlayerTasks>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (HowManyNewTasksWeNeedToGenerate() > 0) GenerateTasks();
    }

    private void GenerateTasks()
    {
        var maxCargoCapacity = spaceShipController.MaxCargoCapacity;
		
        var taskType = GetRandomTaskType();
        var taskDescription = GetTaskDescription(taskType);

		_planets = MapController.GetPlanets().ToList();
        var planetFrom = _planets[Random.Range(0, _planets.Count - 1)];
        var planetTo = _planets.Where(x => x != planetFrom).ToList()[Random.Range(0, _planets.Count - 2)];
       
        CargoItem item = GetComponent<CargoAssigner>().GetItemForType(TaskType.POWER_GENERATION);
        var newTask = new PlayerTasks
        {
            CargoName = taskDescription,
            CargoUnits = Random.Range(1, maxCargoCapacity),
            PlanetFrom = planetFrom,
            PlanetTo = planetTo,
            StartDateIssued = 0,
            DeliveryTick = 10,
            CargoItem = item,
        };

        availableTasks.Add(newTask);
        Debug.Log($"new task name {taskDescription}");

        onTaskCreate.Invoke(newTask);
    }

	private static TaskType GetRandomTaskType()
	{
		var taskTypesArray = typeof(TaskType).GetEnumValues();
		var randomCargoTypeIndex = Random.Range(0, taskTypesArray.Length - 1);
		TaskType taskType = (TaskType)taskTypesArray.GetValue(randomCargoTypeIndex);
		return taskType;
	}

	private static string GetTaskDescription(TaskType taskType)
    {
		return TaskDescriptions.GetDescriptions()[taskType];

	}
	private int HowManyNewTasksWeNeedToGenerate()
    {
        return _maxNumberOfAvailableTasks - availableTasks.Count;
    }
}