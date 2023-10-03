using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        }
        else
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
        var item = Instantiate(listItemPrefab, transform, false);
        var liController = item.GetComponent<ListItemUIController>();
        liController.SetListItem(task);
        _tasks.Add(task, item);

        SortListItemsByDate();
    }

    public void removeListItem(PlayerTask task)
    {
        _tasks[task].GetComponent<ListItemUIController>().OnPointerExit(null);
        Destroy(_tasks[task]);
        _tasks.Remove(task);
        
        SortListItemsByDate();
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

    public void SortListItemsByDate()
    {
        var listItemsSorted = _tasks
            .OrderBy(kv => kv.Key.Deadline)
            .Reverse()
            .Select(x => x.Value)
            .ToList();
        for (int i = 0; i < listItemsSorted.Count; i++)
        {
            listItemsSorted[i].transform.SetSiblingIndex(i);
        }
        
    }
}