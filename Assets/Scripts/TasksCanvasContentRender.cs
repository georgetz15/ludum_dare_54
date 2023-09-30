using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class TasksCanvasContentRender : MonoBehaviour
{
    // prefab component
    [SerializeReference] public GameObject ListItem;
    
    // data from an other controller
    [SerializeReference] public GameObject taskController;
    
    // Start is called before the first frame update
    void Start()
    {
        var item = Instantiate(ListItem);
        item.transform.parent = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onTaskCreate(PLayerTasks pLayerTask)
    {
    }
}
