using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class ScrollViewContentController : MonoBehaviour
{
    [SerializeField] private GameObject listItemPrefab;

    private Dictionary<PLayerTasks, Transform> _tasks;
    // Start is called before the first frame update
    void Awake()
    {
        _tasks = new Dictionary<PLayerTasks, Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createListItem(PLayerTasks task)
    {
        var item = Instantiate(listItemPrefab, transform, true);
        var liController = item.GetComponent<ListItemUIController>();
        liController.setListItem(task);
        _tasks.Add(task, item.transform);
    }

    public void removeListItem(PLayerTasks task)
    {
        Destroy(_tasks[task]);
        _tasks.Remove(task);
    }
}
