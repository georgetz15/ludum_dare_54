using System.Collections;
using System.Collections.Generic;
using Models;
using TMPro;
using UnityEngine;

public class TasksCanvasContentRender : MonoBehaviour
{
    // prefab component
    [SerializeReference] public TMP_Text ListItem;
    
    // data from an other controller
    [SerializeReference] public GameObject taskController;
    
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onTaskCreate(PLayerTasks pLayerTask)
    {
        var item = Instantiate(ListItem);
        item.transform.parent = gameObject.transform;
        item.text = $"{pLayerTask.CargoUnits} ({pLayerTask.CargoName})";
    }
}
