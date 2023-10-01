using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class ScrollViewContentController : MonoBehaviour
{
    [SerializeField] private GameObject listItemPrefab;

    private Dictionary<PlayerTask, Transform> _tasks;
    // Start is called before the first frame update
    void Awake()
    {
        _tasks = new Dictionary<PlayerTask, Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createListItem(PlayerTask task)
    {
        var item = Instantiate(listItemPrefab, transform, true);
        var liController = item.GetComponent<ListItemUIController>();
        liController.SetListItem(task);
        _tasks.Add(task, item.transform);
    }

    public void removeListItem(PlayerTask task)
    {
        Destroy(_tasks[task]);
        _tasks.Remove(task);
    }
}
