using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;
    public List<CargoItem> Items = new List<CargoItem>();

	
	public Transform InventoryGridTransform;
	public GameObject ItemPrefab;
	
	private int capacity;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		capacity = SpaceShipController.Instance.MaxCargoCapacity;
		LoadInventory();
	}

	public void AddItem(CargoItem item)
	{
		if (Items.Count > capacity)
		{
			Debug.LogError("Incorrect handling for quest delegation!! Check the correct storage amount");
			return;
		}
		Items.Add(item);
	}

	public void RemoveItem(CargoItem item)
	{
		Items.Remove(item);
	}
	
	public void ToggleCallback(bool b)
	{
		if (b)
		{
			LoadInventory();
		}
	}
	public void LoadInventory()
	{
		int freeSlots = capacity - Items.Count;

		foreach (Transform item in InventoryGridTransform)
		{
			Destroy(item.gameObject);
		}

		foreach (var item in Items)
		{
			GameObject obj = Instantiate(ItemPrefab, InventoryGridTransform);
			var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

			itemIcon.sprite = item.icon;
		}

		for (int i = 0; i < freeSlots; i++)
		{
			Instantiate(ItemPrefab, InventoryGridTransform);
		}
	}
}
