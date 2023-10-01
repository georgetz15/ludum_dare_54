using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class ScrollViewContentController : MonoBehaviour
{
    [SerializeField] private GameObject listItemPrefab;

    private Dictionary<PlayerTask, GameObject> _tasks = new();
    public static ScrollViewContentController Instance;

	// Start is called before the first frame update
	void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void UpdateTask(PlayerTask task)
    {
        if (!_tasks.ContainsKey(task)) return;

        var listController = _tasks[task].GetComponent<ListItemUIController>();
        if (listController is null) return;

        listController.SetListItem(task);
	}
    public void createListItem(PlayerTask task)
    {
        var item = Instantiate(listItemPrefab, transform, true);
        var liController = item.GetComponent<ListItemUIController>();
        liController.SetListItem(task);
        _tasks.Add(task, item);
    }

    public void removeListItem(PlayerTask task)
    {
        _tasks[task].GetComponent<ListItemUIController>().OnPointerExit(null);
        Destroy(_tasks[task]);
        _tasks.Remove(task);
    }
    

    public void MakeTasksInactive(List<PlayerTask> tasks)
    {
		foreach (var task in tasks)
		{
			task.Status = Tasks.TaskStatus.INACTIVE;
			_tasks[task].GetComponent<ListItemUIController>().SetStatus(task);

		}
	}

    public void MakeTasksAvailable(List<PlayerTask> tasks)
    {
        foreach (var task in tasks)
        {
            task.Status = Tasks.TaskStatus.AVAILABLE;
            _tasks[task].GetComponent<ListItemUIController>().SetStatus(task);

		}
    }
}