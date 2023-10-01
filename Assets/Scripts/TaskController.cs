using System.Collections.Generic;
using System.Linq;
using Models;
using Tasks;
using UnityEngine;
using UnityEngine.Events;

public class TaskController : MonoBehaviour
{
    public static TaskController Instance;
    [field: SerializeReference] public SpaceShipController spaceShipController { get; set; }

    [SerializeField] public UnityEvent<PlayerTask> onTaskCreate;
    [SerializeField] private UnityEvent<PlayerTask> onTaskComplete = new();


    private readonly int _maxNumberOfAvailableTasks = 12;

    private readonly List<PlayerTask> _tasks = new();
    private readonly HashSet<PlayerTask> _activeTasks = new();

    private List<GameObject> _planets;
    
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (onTaskCreate == null)
            onTaskCreate = new UnityEvent<PlayerTask>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (HowManyNewTasksWeNeedToGenerate() > 0) GenerateTask();
    }

    private void GenerateTask()
    {
        var maxCargoCapacity = spaceShipController.MaxCargoCapacity;

        var taskType = GetRandomTaskType();
        var taskDescription = GetTaskDescription(taskType);

        _planets = MapController.GetPlanets().ToList();
        var planetFrom = _planets[Random.Range(0, _planets.Count - 1)];
        var planetTo = _planets.Where(x => x != planetFrom).ToList()[Random.Range(0, _planets.Count - 2)];

        var item = GetComponent<CargoAssigner>().GetItemForType(taskType);
        var newTask = new PlayerTask
        {
            CargoName = taskDescription,
            CargoUnits = Random.Range(1, maxCargoCapacity),
            PlanetFrom = planetFrom,
            PlanetTo = planetTo,
            StartDateIssued = 0,
            DeliveryTick = 10,
            CargoItem = item,
            Reward = Random.Range(2, 10) * 10,
            Status = TaskStatus.INACTIVE
        };
        _tasks.Add(newTask);

        Debug.Log($"new task name {taskDescription}");

        onTaskCreate.Invoke(newTask);
    }

    private static TaskType GetRandomTaskType()
    {
        var taskTypesArray = typeof(TaskType).GetEnumValues();
        var randomCargoTypeIndex = Random.Range(0, taskTypesArray.Length - 1);
        var taskType = (TaskType)taskTypesArray.GetValue(randomCargoTypeIndex);
        return taskType;
    }

    private static string GetTaskDescription(TaskType taskType)
    {
        return TaskDescriptions.GetDescriptions()[taskType];
    }

    public TaskErrorCode CanAcceptMission(PlayerTask mission)
    {
        if (mission == null) return TaskErrorCode.INVALID_TASK;

		var sc = SpaceShipController.Instance;
		if (mission.CargoUnits >= sc.MaxCargoCapacity - sc.Cargo)
			return TaskErrorCode.INSUFFICIENT_SPACE;

        if (mission.PlanetFrom != sc.CurrentPlanet || sc.IsTravelling) 
            return TaskErrorCode.INVALID_START_PLANET;

        return TaskErrorCode.OK;	
    }

    private int HowManyNewTasksWeNeedToGenerate()
    {
        return _maxNumberOfAvailableTasks - _tasks.Count;
    }

    public void SetTaskActive(PlayerTask task)
    {
        task.Status = TaskStatus.ACTIVE;
        _activeTasks.Add(task);
    }

    public void CompleteTask(PlayerTask task)
    {
        _tasks.Remove(task);
        _activeTasks.Remove(task);

        onTaskComplete.Invoke(task);
    }

    public void DisableNonActiveTasks()
    {
        var scrollViewCtrl = ScrollViewContentController.Instance;
        if (scrollViewCtrl is null) return;
	    var nonActiveTasks = _tasks.Where(task =>!_activeTasks.Contains(task)).ToList();
        scrollViewCtrl.MakeTasksInactive(nonActiveTasks);
	}

    public void EnableTasksForPlanet(GameObject planet)
    {
        var tasksForPlanet = _tasks.Where(task => (task.PlanetFrom == planet && !_activeTasks.Contains(task))).ToList();
		var scrollViewCtrl = ScrollViewContentController.Instance;
		if (scrollViewCtrl is null) return;
        scrollViewCtrl.MakeTasksAvailable(tasksForPlanet);
	}

    public List<PlayerTask> GetTasksWithPlanetFrom(GameObject planetFrom)
    {
        return _tasks.Where(task => task.PlanetFrom == planetFrom).ToList();
    }

    public void CompleteAllTasksWithDestination(Transform planetTo)
    {
        foreach (var task in _activeTasks.Where(task => task.PlanetTo == planetTo.gameObject).ToList())
            CompleteTask(task);
    }
}