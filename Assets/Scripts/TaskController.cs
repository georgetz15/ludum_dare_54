using System.Collections.Generic;
using System.Linq;
using Tasks;
using UnityEngine;
using UnityEngine.Events;
using Models;

public class TaskController : MonoBehaviour
{
    [field: SerializeReference] public SpaceShipController spaceShipController { get; set; }

    [SerializeField] public UnityEvent<PlayerTask> onTaskCreate;

    
    private readonly int _maxNumberOfAvailableTasks = 12;
    public static TaskController Instance;

    private List<GameObject> _planets;

    [SerializeField] public Dictionary<GameObject, List<PlayerTask>> availableTasks = new();

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
	
		if (onTaskCreate == null)
            onTaskCreate = new UnityEvent<PlayerTask>();
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
        var newTask = new PlayerTask
        {
            CargoName = taskDescription,
            CargoUnits = Random.Range(1, maxCargoCapacity),
            PlanetFrom = planetFrom,
            PlanetTo = planetTo,
            StartDateIssued = 0,
            DeliveryTick = 10,
            CargoItem = item,
        };

        AddTask(planetFrom, newTask);
        Debug.Log($"new task name {taskDescription}");

        onTaskCreate.Invoke(newTask);
    }

    private void AddTask(GameObject planetFrom, PlayerTask newTask)
    {
		if (availableTasks.ContainsKey(planetFrom))
		{
			availableTasks[planetFrom].Add(newTask);
		}
		else
		{
			List<PlayerTask> newList = new List<PlayerTask>();
			newList.Add(newTask);
			availableTasks.Add(planetFrom, newList);
		}
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

    public bool CanAcceptMission(PlayerTask mission)
    {
		if (mission == null) return false;
		return CanAcceptMission(mission.CargoUnits);
    }

    public bool CanAcceptMission(int cargoQty)
    {
        var sc = SpaceShipController.Instance;
        if (cargoQty >= sc.MaxCargoCapacity - sc.Cargo)
        {
            return false;
        }

        return true;
    }

	private int HowManyNewTasksWeNeedToGenerate()
    {
        return _maxNumberOfAvailableTasks - availableTasks.Count;
    }
}