using UnityEngine;
using TMPro;

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

	public void DisplayMsg(string msg)
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
