using UnityEngine;
using TMPro;
using System.Timers;
using System.Collections;

public class MessageBox : MonoBehaviour
{
	public TMP_Text textComponent;
	public static MessageBox Instance;
	public GameObject messageBoxContainer;

	private void Start()
	{
		if (Instance == null)
		{
			Instance = this;
		} else
		{
			Destroy(gameObject);
		}
	}

	IEnumerator HideAfterDelay()
	{
		float delay = 4.0f; 
		yield return new WaitForSeconds(delay);
		messageBoxContainer.SetActive(false);
	}

	public void DisplayMsg(string msg)
	{
		SetText(msg);
		var inventoryController = InventoryController.Instance;
		inventoryController.CloseInventoryUI();
		messageBoxContainer.SetActive(true);
		StartCoroutine(HideAfterDelay());
	}

	public void DisplayMsgWithoutAutohide(string msg)
	{
		SetText(msg);
		var inventoryController = InventoryController.Instance;
		inventoryController.CloseInventoryUI();
		messageBoxContainer.SetActive(true);
	}
	
	private void SetText(string message)
	{
		if (textComponent == null)
		{
			return;
		}
		textComponent.text = message;
	}
}
