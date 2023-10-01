using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Models;
using UnityEngine.UI;
using ScriptableObjects;

public class ListItemUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text fromPlanetText;
    [SerializeField] private TMP_Text toPlanetText;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text deadlineText;
	[SerializeField] private CargoItem cargoItem;
    [SerializeField] private int cargoQuantity;

	private void Awake()
	{
		var btn = GetComponent<Button>();
		btn.onClick.AddListener(delegate
		{
            var invCtrl = InventoryController.Instance;
			if (invCtrl is null) return;
            var taskCtrl = TaskController.Instance;
            if (taskCtrl is null) return;

            if (!taskCtrl.CanAcceptMission(cargoQuantity))
            {
                return;
            }
            invCtrl.AddItem(cargoItem, cargoQuantity);
		});
	}
	public void SetListItem(PlayerTask task)
    {
        titleText.text = task.CargoName;
        fromPlanetText.text = $"From: {task.PlanetFrom.name}";
        toPlanetText.text = $"To: {task.PlanetTo.name}";
        rewardText.text = $"Reward: {100} credits";
        deadlineText.text = $"Deadline: {task.DeliveryTick} parsecs";
        cargoItem = task.CargoItem;
        cargoQuantity = task.CargoUnits;
    }
}
