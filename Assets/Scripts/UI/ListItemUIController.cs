using Models;
using Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ListItemUIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
	    	var messageBox = MessageBox.Instance;
            switch (taskCtrl.CanAcceptMission(_task))
            {
                case TaskErrorCode.INSUFFICIENT_SPACE:
                    messageBox.DisplayMsg("Your space is too limited for this cargo, sorry...");
                    break;
                case TaskErrorCode.INVALID_START_PLANET:
					messageBox.DisplayMsg("You are not standing at the start planet for this quest...");
                    break;
                case TaskErrorCode.OK:
					invCtrl.AddItem(cargoItem, cargoQuantity);
                    break;
                default:
                    break;
            }
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
    }

    private void OnHoverEnter()
    {
        _task.PlanetFrom.GetComponent<PlanetController>()?.ShowGlow(PlanetController.GlowType.From);
        _task.PlanetTo.GetComponent<PlanetController>()?.ShowGlow(PlanetController.GlowType.To);
    }

    private void OnHoverExit()
    {
        _task.PlanetFrom.GetComponent<PlanetController>()?.HideGlow();
        _task.PlanetTo.GetComponent<PlanetController>()?.HideGlow();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit();
    }
}