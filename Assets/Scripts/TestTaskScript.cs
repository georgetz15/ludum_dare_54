using System.Collections;
using System.Collections.Generic;
using Tasks;
using UnityEngine;

[System.Serializable]
public class CargoTypeToScriptableObject
{
	public TaskType cargoType;
	public List<CargoItem> cargoItemPrefabs;
}

public class TestTaskScript : MonoBehaviour
{
    public List<CargoTypeToScriptableObject> cargoMappings;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
