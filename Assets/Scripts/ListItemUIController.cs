using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Models;

public class ListItemUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text fromPlanetText;
    [SerializeField] private TMP_Text toPlanetText;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text deadlineText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setListItem(PLayerTasks task)
    {
        titleText.text = task.CargoName;
        fromPlanetText.text = $"From: {task.PlanetFrom.name}";
        toPlanetText.text = $"From: {task.PlanetTo.name}";
        rewardText.text = $"Reward: {100} credits";
        deadlineText.text = $"Deadline: {task.DeliveryTick}";
    }
}
