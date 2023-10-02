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
	[SerializeField] private TMP_Text cargoText;
	[SerializeField] private TMP_Text statusText;

	[SerializeField] private CargoItem cargoItem;
    [SerializeField] private int cargoQuantity;

    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
	[SerializeField] private Sprite availableSprite;

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
					messageBox.DisplayMsg("You are not at the start planet for this quest...");
                    break;
                case TaskErrorCode.OK:
                    taskCtrl.SetTaskActive(_task);
					SetStatus(_task);
					invCtrl.AddItem(cargoItem, cargoQuantity);
                    break;
				case TaskErrorCode.ALREADY_ACTIVE:
				default:
                    break;
            }
		});
	}
	
    public void SetStatus(PlayerTask task)
    {   
        var state = task.Status;
        var image = GetComponent<Image>();
        switch (state)
        {
            case TaskStatus.AVAILABLE:
                image.sprite = availableSprite;
                break;
            case TaskStatus.ACTIVE:
                image.sprite = activeSprite;
				break;
            case TaskStatus.INACTIVE:
                image.sprite = inactiveSprite;
                break;
            default:
                break;
        }
		statusText.text = $"Status: {GetTaskStatusStr()}";
	}

	public void SetListItem(PlayerTask task)
    {
        _task = task;

        titleText.text = task.CargoName;
        fromPlanetText.text = $"From: {task.PlanetFrom.name}";
        toPlanetText.text = $"To: {task.PlanetTo.name}";
        rewardText.text = $"Reward: {task.Reward} credits";
        deadlineText.text = $"Deadline: {task.Deadline} days";
        cargoText.text = $"Cargo: {task.CargoUnits} units";
        statusText.text = $"Status: {GetTaskStatusStr()}";

        cargoItem = task.CargoItem;
        cargoQuantity = task.CargoUnits;
    }

    private string GetTaskStatusStr()
    {
        switch (_task.Status)
        {
            case TaskStatus.ACTIVE:
                return "Active";
            case TaskStatus.AVAILABLE:
            case TaskStatus.INACTIVE:
                return "Inactive";
            default:
                return "";
        }
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