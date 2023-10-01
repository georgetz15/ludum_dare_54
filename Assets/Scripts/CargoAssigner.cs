using System.Collections;
using System.Collections.Generic;
using Tasks;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class CargoTypeToScriptableObject
{
	public TaskType taskType;
	public List<CargoItem> cargoObjects;
}

public class CargoAssigner : MonoBehaviour
{
    public List<CargoTypeToScriptableObject> cargoMappings;

	public CargoItem GetItemForType(TaskType taskType)
	{
		foreach (CargoTypeToScriptableObject item in cargoMappings)
		{
			if (item.taskType == taskType)
			{
				// Return random cargo item from list
				return item.cargoObjects[Random.Range(0, item.cargoObjects.Count - 1)];
			}
		}

		return null;
	}
}
