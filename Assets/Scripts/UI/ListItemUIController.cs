using Models;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ListItemUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text fromPlanetText;
    [SerializeField] private TMP_Text toPlanetText;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private TMP_Text deadlineText;
    [SerializeField] private CargoItem cargoItem;
    [SerializeField] private int cargoQuantity;
    private PlayerTask _task;

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
                var messageBox = MessageBox.Instance;
                if (messageBox is null)
                {
                    messageBox = new MessageBox();
                };

                messageBox.DisplayMsg("Your space is too limited for this cargo, sorry...");
                return;
            }
            invCtrl.AddItem(cargoItem, cargoQuantity);
		});
	}
	public void SetListItem(PlayerTask task)
    {
        _task = task;

        titleText.text = task.CargoName;
        fromPlanetText.text = $"From: {task.PlanetFrom.name}";
        toPlanetText.text = $"To: {task.PlanetTo.name}";
        rewardText.text = $"Reward: {100} credits";
        deadlineText.text = $"Deadline: {task.DeliveryTick} parsecs";
        cargoItem = task.CargoItem;
        cargoQuantity = task.CargoUnits;

        // Add hover triggers
        EventTrigger.Entry hoverEntry = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerEnter
        };
        hoverEntry.callback.AddListener(_ => { OnHoverEnter(); });
        
        EventTrigger.Entry hoverExit = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerExit
        };
        hoverExit.callback.AddListener(_ => { OnHoverExit(); });
        
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
        eventTrigger.triggers.Add(hoverEntry);
        eventTrigger.triggers.Add(hoverExit);
    }

    public void OnHoverEnter()
    {
        _task.PlanetFrom.GetComponent<PlanetController>()?.ShowGlow(PlanetController.GlowType.From);
        _task.PlanetTo.GetComponent<PlanetController>()?.ShowGlow(PlanetController.GlowType.To);
    }

    public void OnHoverExit()
    {
        _task.PlanetFrom.GetComponent<PlanetController>()?.HideGlow();
        _task.PlanetTo.GetComponent<PlanetController>()?.HideGlow();
    }
}