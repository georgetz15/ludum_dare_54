using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cargo Item", menuName = "CargoItem/Create New Cargo Item")]
public class CargoItem : ScriptableObject
{
	public int id;
	public string itemName;
	public Sprite icon;
}

