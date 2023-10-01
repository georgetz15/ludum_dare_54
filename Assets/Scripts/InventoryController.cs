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
	
	private void AssertCapacity()
	{
		if (Items.Count >= capacity)
		{
			Debug.LogError("Incorrect handling for quest delegation!! Check the correct storage amount");
			return;
		}
	}

	public void AddItem(CargoItem item)
	{
		AssertCapacity();
		Items.Add(item);
		LoadInventory();
	}

	public void AddItem(CargoItem item, int qty)
	{
		for (int i = 0; i < qty; i++)
		{
			AssertCapacity();
			Items.Add(item);
		}
		LoadInventory();
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
			CargoItemController itemIconController = obj.GetComponent<CargoItemController>();
			itemIconController.SetImage(item.icon);
		}

		for (int i = 0; i < freeSlots; i++)
		{
			Instantiate(ItemPrefab, InventoryGridTransform);
		}
	}
}
